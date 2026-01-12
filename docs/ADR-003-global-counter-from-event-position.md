# ADR-003 — Global Counter Derived from Event Position

## Status
Accepted

## Context
Maintaining a single-row global counter (`UPDATE counter SET value=value+1`) becomes a write hotspot under high concurrency. The design already requires an append-only event store with a monotonic global ordering.

The API requires a global counter value for both:

- global increment (`POST /counter`)
- user increment (`POST /counter/{userId}`)

## Decision
Use the event store’s identity position (`write.Events.Position`) as the global counter value:

- Each increment appends a `CounterIncremented` event (optional `UserId`).
- `globalValue` is the inserted row’s `Position` for the request.
- `GET /counter` reads `MAX(Position)` (via `read.GlobalCounter` view filtered by `EventType='CounterIncremented'`).

## Consequences
- **Pros**
  - Avoids a single hot counter row
  - Global value is naturally monotonic and durable
  - Simplifies global reads (view over events)
  - Works uniformly for global-only and user increments
- **Cons**
  - If many event types are introduced, filtering is required
  - `globalValue` represents event sequence rather than a separate “business counter” if semantics diverge

## References
- [DOC-04 API Contract](DOC-04-api-contract.md)
- [DOC-05 Persistence Design](DOC-05-persistence-design.md)
