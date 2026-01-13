# DOC-02 — Product Goals, Constraints, Non-Goals

## Goals

1. **Horizontal scalability at the API tier**  
   App instances are stateless; counter state is stored durably in SQL.

2. **Durable persistence and auditability**  
   Every increment is represented as an append-only event.

3. **Strong consistency for increment**  
   Use transactional projections: event + projection update in the same SQL transaction.

4. **Future-proof for per-user counts**  
   The write model includes `UserId` and the read model includes per-user counters.

5. **Environment parity**  
   Same code paths work:
   - locally with SQL Server LocalDB
   - in production with Azure SQL

## Constraints

- Minimal APIs (no new Controllers)
- Clean Architecture + Vertical Slices (Milan Jovanović style)
- CQRS without MediatR
- Event Sourcing + Transactional projections
- SQL Server family only (LocalDB + Azure SQL)
- Atomic writes for increment

## Non-Goals (explicitly out of scope)

- UI/client changes beyond what is required to call the new API
