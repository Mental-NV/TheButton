# ADR-001 — CQRS without MediatR (Handler-per-UseCase)

## Status
Accepted

## Context
The project requires a CQRS approach aligned with Milan Jovanović’s template and explicitly forbids MediatR. The desired style is Clean Architecture + Vertical Slices with Minimal APIs.

## Decision
Implement CQRS by defining per-use-case command/query types and handler classes, and invoke handlers directly from Minimal API endpoints via DI.

- Each use case has:
  - a command or query record
  - a handler class with `Handle(...)`
  - an endpoint that performs minimal validation and mapping
- No mediator abstraction is introduced.

## Consequences
- **Pros**
  - Low magic and fewer dependencies
  - Very clear execution flow (endpoint → handler → repository)
  - Easier for agents to generate predictable code
- **Cons**
  - More explicit wiring (handlers registered in DI)
  - Some duplication across use cases (acceptable in vertical slices)

## References
- [DOC-01 Agent Operating Instructions](DOC-01-agent-instructions.md)
- [DOC-03 Architecture Overview](DOC-03-architecture-overview.md)
