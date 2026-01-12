# ADR-002 — Variant A Transactional Projections (Strong Consistency)

## Status
Accepted

## Context
The system must support horizontal scaling at the API tier while keeping writes atomic and strongly consistent. The design must follow event sourcing and CQRS, remain compatible with Azure SQL and SQL Server LocalDB, and support two write modes:

- Global increment (no user)
- User increment (global + per-user)

Idempotency must be safe across retries and must not collide across different operations.

## Decision
Use **Variant A: Transactional projections** in a single database:

- Append the event to `write.Events` and (when applicable) update the projection `read.UserCounters` **in the same SQL transaction**.
- Persist idempotency outcomes in `write.Commands` scoped by:
  - `Operation` (`GlobalIncrement` or `UserIncrement`)
  - optional `UserId`
  - `IdempotencyKey` (header value)

There is **one** increment event type (`CounterIncremented`) with **optional `UserId`**.

## Consequences
- **Pros**
  - Strong consistency (“read your write”)
  - Operational simplicity (one database)
  - Clear correctness boundary (single transaction)
  - Idempotency is explicit and safe across operations
- **Cons**
  - Reads and writes share a database (scales vertically first)
  - Future extreme scale may require sharding or an async projection model (Variant B)

## References
- [DOC-02 Product Goals, Constraints, Non-Goals](DOC-02-goals-constraints-non-goals.md)
- [DOC-04 API Contract](DOC-04-api-contract.md)
- [DOC-05 Persistence Design](DOC-05-persistence-design.md)
- [DOC-07 Integration Testing Strategy](DOC-07-integration-testing.md)
