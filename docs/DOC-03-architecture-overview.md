# DOC-03 — Architecture Overview

## Layered architecture (Clean Architecture)

- **TheButton.Api**
  - Minimal APIs and endpoint mapping
  - API versioning configuration (Asp.Versioning)
  - Request/response DTOs
  - Composition root (DI wiring)

- **TheButton.Application**
  - Commands/queries and handlers (CQRS)
  - Abstractions (interfaces) for persistence and other dependencies
  - Validation logic (lightweight; can be in endpoints or handlers)

- **TheButton.Domain**
  - Domain events and primitives
  - Keep domain small and purposeful (avoid unnecessary complexity)

- **TheButton.Infrastructure**
  - EF Core DbContext and migrations
  - SQL repository implementations
  - Any SQL-specific logic

## CQRS (no MediatR)

Each use case has:

- `Command` or `Query` record
- `Handler` class with `Handle(...)` method
- Endpoint calls handler directly (DI resolves handler)

## Vertical Slices
- Feature folders follow vertical slice structure: `Features/<Version>/<Area>/<UseCase>/`

### Example folder structure

```text
src/
  TheButton.Api/
    Features/
      V3/
        Counter/
          IncrementGlobal/
            Endpoint.cs
            Response.cs
          IncrementUser/
            Endpoint.cs
            Response.cs
          GetGlobal/
            Endpoint.cs
            Response.cs
          GetUser/
            Endpoint.cs
            Response.cs
  TheButton.Application/
    Abstractions/
      ICounterWriter.cs
      ICounterReadRepository.cs
    Counter/
      V3/
        IncrementGlobal/
          GlobalIncrementCommand.cs
          GlobalIncrementHandler.cs
          GlobalIncrementResult.cs
        IncrementUser/
          UserIncrementCommand.cs
          UserIncrementHandler.cs
          UserIncrementResult.cs
        GetGlobal/
          GetGlobalQuery.cs
          GetGlobalQueryHandler.cs
        GetUser/
          GetUserCountersQuery.cs
          GetUserCountersQueryHandler.cs

  TheButton.Domain/
    Events/
      CounterIncremented.cs

  TheButton.Infrastructure/
    Persistence/
      TheButtonDbContext.cs
      Entities/
      Configurations/
    Counter/
        SqlCounterWriter.cs
        SqlCounterReadRepository.cs
```

## API versioning (Manual)

- Expose versions via URL segments: `/api/v2/...`, `/api/v3/...`.
- Implement endpoints using standard `MapGroup` with explicit version segments.
- This provides maximum control and simplicity without external library overhead.

This ensures:
- The runtime routes match [DOC-04 API Contract](DOC-04-api-contract.md).
- OpenAPI/Scalar groups endpoints intuitively based on the route structure.

## Transactional projections (strong consistency)

- A single database contains:
  - `write` schema: event store + idempotency
  - `read` schema: projections
- Write operations execute a **single SQL transaction** that:
  - enforces idempotency
  - updates projections when applicable
  - appends the event
  - stores the command result

## References

- [DOC-04 API Contract](DOC-04-api-contract.md)
- [DOC-05 Persistence Design](DOC-05-persistence-design.md)
