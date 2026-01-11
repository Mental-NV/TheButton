# ADR-002 — Variant A Transactional Projections (Strong Consistency)

## Status
Accepted

## Context
The system must provide a scalable increment mechanism that supports horizontal API scaling while keeping writes atomic. The design must follow event sourcing and CQRS, and remain compatible with Azure SQL and local SQL Server.

## Decision
Use **Variant A: Transactional projections** in a single database:

- Append the event to `write.Events` and update the read projection (`read.UserCounters`) **in the same SQL transaction**.
- Persist idempotency outcomes in `write.Commands` to safely handle client retries.

## Consequences
- **Pros**
  - Strong consistency (“read your write”)
  - Operational simplicity (one database)
  - Easier to implement and test (single transaction boundary)
- **Cons**
  - Read and write share one database (scales vertically first)
  - If extreme scale is required, future work may introduce sharding or Variant B

## References
- [DOC-02 Goals, Constraints, Non-Goals](../DOC-02-goals-constraints-non-goals.md)
- [DOC-05 Persistence Design](../DOC-05-persistence-design.md)
- [DOC-07 Integration Testing Strategy](../DOC-07-integration-testing.md)
