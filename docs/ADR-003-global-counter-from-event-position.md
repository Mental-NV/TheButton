# ADR-003 — Global Counter Derived from Event Position

## Status
Accepted

## Context
Maintaining a single-row global counter (`UPDATE counter SET value=value+1`) can become a write hotspot under high concurrency. The system already requires an append-only event store with a monotonic ordering.

## Decision
Use the event store’s identity position (`write.Events.Position`) as the global counter value:

- Each increment appends a `CounterIncremented` event.
- `globalValue` is the inserted row’s `Position` (or `MAX(Position)` for reads).
- Provide a `read.GlobalCounter` view that returns `MAX(Position)` filtered by `EventType='CounterIncremented'`.

## Consequences
- **Pros**
  - Avoids a single hot counter row
  - Global value is naturally monotonic and durable
  - Simplifies global reads (view over events)
- **Cons**
  - If multiple event types are introduced, filtering is required
  - Global value represents event sequence, not necessarily “business counter” if semantics change

## References
- [DOC-04 API Contract](../DOC-04-api-contract.md)
- [DOC-05 Persistence Design](../DOC-05-persistence-design.md)
