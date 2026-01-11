# DOC-03 — Architecture Overview

## Layered architecture (Clean Architecture)

- **TheButton.Api**
  - Minimal APIs and endpoint mapping
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

### Example folder structure

```text
src/
  TheButton.Api/
    Features/
      Counter/
        Increment/
          Endpoint.cs
          Request.cs
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
      Increment/
        IncrementCommand.cs
        IncrementCommandHandler.cs
        IncrementResult.cs
      GetGlobal/
        GetGlobalQuery.cs
        GetGlobalQueryHandler.cs
      GetUser/
        GetUserCounterQuery.cs
        GetUserCounterQueryHandler.cs

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

## Variant A: Transactional projections (strong consistency)

- A single database contains:
  - `write` schema: event store + idempotency
  - `read` schema: projections
- The increment command executes a **single SQL transaction** that:
  - enforces idempotency
  - updates projections
  - appends the event
  - stores the command result

## References

- DOC-04 — API Contract
- DOC-05 — Persistence Design
