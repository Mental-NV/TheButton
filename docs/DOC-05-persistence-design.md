# DOC-05 — Persistence Design (Event Store + Transactional Projections)

This design implements Event Sourcing with **Variant A transactional projections**: the event append, projection update, and idempotency record are committed in **one SQL transaction**.

## Schemas

- `write`: event store + idempotency
- `read`: projections (query-optimized)

## Event model

There is **one global increment event** type with an **optional `UserId`**.

- Global increment (`POST /counter`): event has `UserId = NULL`.
- User increment (`POST /counter/{userId}`): event has `UserId = <userId>` and a per-user monotonic `UserVersion`.

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

### `write.Commands` (idempotency)

Idempotency is scoped by operation and optional user.

| Column | Type | Notes |
|---|---|---|
| `Operation` | `NVARCHAR(50)` | `GlobalIncrement` or `UserIncrement` |
| `UserId` | `UNIQUEIDENTIFIER NULL` | NULL for global increment |
| `IdempotencyKey` | `NVARCHAR(100)` | From `Idempotency-Key` header |
| `CreatedUtc` | `DATETIME2` | |
| `ResultJson` | `NVARCHAR(MAX)` | Cached response payload |

**Constraints / indexes**
- Unique: `(Operation, UserId, IdempotencyKey)`
  - Prevents collisions when the same idempotency key is reused across different operations.

### `read.UserCounters` (projection)

| Column | Type | Notes |
|---|---|---|
| `UserId` | `UNIQUEIDENTIFIER` | PK |
| `Value` | `BIGINT` | NOT NULL |

### `read.GlobalCounter` (view)

```sql
SELECT ISNULL(MAX(Position), 0) AS GlobalValue
FROM write.Events
WHERE EventType = 'CounterIncremented';
```

## Atomic write algorithms (single SQL transaction)

### A) Global increment (`POST /counter`)

Transaction steps:

1. If `write.Commands` contains `(Operation='GlobalIncrement', UserId=NULL, IdempotencyKey=...)`, return stored `ResultJson`.
2. Insert `CounterIncremented` event into `write.Events` with `UserId=NULL`, `UserVersion=NULL`, capture `Position` as `globalValue`.
3. Insert into `write.Commands` the cached result `{ globalValue }`.
4. Commit.

### B) User increment (`POST /counter/{userId}`)

Transaction steps:

1. If `write.Commands` contains `(Operation='UserIncrement', UserId=<userId>, IdempotencyKey=...)`, return stored `ResultJson`.
2. Upsert+increment `read.UserCounters.Value` for `<userId>`, producing `userValue`.
3. Insert `CounterIncremented` event into `write.Events` with:
   - `UserId=<userId>`
   - `UserVersion=userValue` (per-user monotonic sequence)
   Capture `Position` as `globalValue`.
4. Insert into `write.Commands` the cached result `{ globalValue, userValue }`.
5. Commit.

## Implementation notes

- Projection upsert should be concurrency-safe (single-statement upsert where feasible).
- Bounded retries may be applied for transient and concurrency exceptions (DOC-08).
- The global counter is derived from event `Position` and does not require a separate hot row (ADR-003).
