# DOC-05 — Persistence Design (Event Store + Transactional Projections)

## Schemas

- `write`: event store + idempotency
- `read`: projections

## Tables

### `write.Events` (append-only)

| Column | Type | Notes |
|---|---|---|
| `Position` | `BIGINT IDENTITY(1,1)` | Global order; clustered PK |
| `EventId` | `UNIQUEIDENTIFIER` | Unique id for event |
| `StreamId` | `NVARCHAR(200)` | `user:{userId}` |
| `StreamVersion` | `INT` | Per-stream version; can equal new userValue |
| `EventType` | `NVARCHAR(100)` | `CounterIncremented` |
| `OccurredUtc` | `DATETIME2` | UTC timestamp |
| `PayloadJson` | `NVARCHAR(MAX)` | JSON payload |

**Constraints / indexes**
- PK clustered: `(Position)`
- Unique: `(StreamId, StreamVersion)`
- Index: `(StreamId, StreamVersion DESC)`

### `write.Commands` (idempotency)

| Column | Type | Notes |
|---|---|---|
| `IdempotencyKey` | `NVARCHAR(100)` | PK or unique |
| `UserId` | `UNIQUEIDENTIFIER` | |
| `CreatedUtc` | `DATETIME2` | |
| `ResultJson` | `NVARCHAR(MAX)` | Cached response |

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

## Atomic increment algorithm (single SQL transaction)

For `POST /counter/increment`:

1. If `write.Commands` contains `IdempotencyKey`, return stored `ResultJson`.
2. Upsert and increment `read.UserCounters` for the `UserId`, producing `userValue`.
3. Insert a new `CounterIncremented` row into `write.Events`, capture `Position` as `globalValue`.
4. Insert `write.Commands` row with `ResultJson = { globalValue, userValue }`.
5. Commit.

## Implementation notes

- The projection increment must be concurrency-safe (single-statement upsert where feasible).
- Bounded retries may be applied for transient or concurrency exceptions (DOC-08).
