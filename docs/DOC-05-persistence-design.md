# DOC-05 — Persistence Design (Event Store + Transactional Projections)

This design implements Event Sourcing with **Variant A transactional projections**: the event append and idempotency record are committed in **one SQL transaction**. User-specific counters are derived directly from the event store.

## Schemas

- `write`: event store + idempotency

## Event model

There is **one global increment event** type with an **optional `UserId`**.

- Global increment (`POST /api/v3/counter`): event has `UserId = NULL`.
- User increment (`POST /api/v3/counter?userId=<guid>`): event has `UserId = <userId>` and a per-user monotonic `UserVersion`.

## Tables

### `write.Events` (append-only)

| Column | Type | Notes |
|---|---|---|
| `Position` | `BIGINT IDENTITY(1,1)` | Global order; clustered PK |
| `EventId` | `UNIQUEIDENTIFIER` | Unique id for event |
| `EventType` | `NVARCHAR(100)` | `CounterIncremented` |
| `OccurredUtc` | `DATETIME2` | UTC timestamp |
| `UserId` | `UNIQUEIDENTIFIER NULL` | NULL for global-only increments |
| `UserVersion` | `BIGINT NULL` | Per-user sequence; NULL for global-only |
| `PayloadJson` | `NVARCHAR(MAX)` | JSON payload; includes optional userId and any future metadata |

**Constraints / indexes**
- PK clustered: `(Position)`
- Unique (filtered): `(UserId, UserVersion)` where `UserId IS NOT NULL`
  - Ensures per-user sequencing is conflict-free and supports high concurrency without a single hot row.
- Index: `(EventType, Position DESC)`
- Index: `(UserId, Position DESC)` (Optimizes user-specific reads)

### `write.Commands` (idempotency)

Idempotency is scoped by operation and optional user.

| Column | Type | Notes |
|---|---|---|
| `Id` | `BIGINT IDENTITY(1,1)` | Clustered PK |
| `Operation` | `NVARCHAR(50)` | `Increment` |
| `UserId` | `UNIQUEIDENTIFIER NULL` | NULL for global increment |
| `IdempotencyKey` | `NVARCHAR(100)` | From `Idempotency-Key` header |
| `CreatedUtc` | `DATETIME2` | |
| `ResultJson` | `NVARCHAR(MAX)` | Cached response payload |

**Constraints / indexes**
- PK clustered: `(Id)`
- Unique: `(Operation, UserId, IdempotencyKey)`
  - Prevents collisions when the same idempotency key is reused across different operations and user contexts.

## Read Projections

### Global Counter (derived)

```sql
SELECT ISNULL(MAX(Position), 0) AS GlobalValue
FROM write.Events
WHERE EventType = 'CounterIncremented';
```

### User Counter (derived)

User counters are calculated on the fly from the event store to reduce complexity and ensure absolute consistency without a separate projection table.

```sql
SELECT ISNULL(MAX(UserVersion), 0) AS UserValue
FROM write.Events
WHERE UserId = @UserId;
```

## Atomic write algorithm (single SQL transaction)

### Unified Increment (`POST /api/v3/counter?userId=...`)

Transaction steps:

1. If `write.Commands` contains `(Operation='Increment', UserId=..., IdempotencyKey=...)`, return stored `ResultJson`.
2. Determine `newUserVersion` (only if `UserId` is provided):
   - `SELECT @currentMax = MAX(UserVersion) FROM write.Events WHERE UserId = @UserId`
   - `SET @newUserVersion = ISNULL(@currentMax, 0) + 1`
3. Insert `CounterIncremented` event into `write.Events` with:
   - `UserId = @UserId`
   - `UserVersion = @newUserVersion`
   - Capture `Position` as `globalValue`.
4. Insert into `write.Commands` the cached result `{ globalValue, userValue: @newUserVersion }`.
5. Commit.

## Implementation notes

- The `UserVersion` calculation utilizes the existing filtered unique index on `(UserId, UserVersion)` for high performance.
- The global counter is derived from event `Position` and does not require a separate hot row ([ADR-003 Global Counter from Event Position](ADR-003-global-counter-from-event-position.md)).
- Removing the `read.UserCounters` table simplifies the system by consolidating the source of truth to `write.Events`.
