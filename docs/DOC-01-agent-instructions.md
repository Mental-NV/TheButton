# DOC-01 — Agent Operating Instructions

## Purpose

This document defines the operating constraints and working practices for coding AI agents implementing the TheButton.API refactor.

## Scope

Implement a scalable production architecture for **TheButton.API** using:

- Minimal APIs
- Clean Architecture + Vertical Slices (Milan Jovanović style)
- CQRS (no MediatR)
- Event Sourcing + **Transactional projections (Variant A)**

## Hard constraints (must follow)

- Do **not** introduce MediatR.
- Do **not** implement new endpoints using Controllers; use Minimal APIs.
- Use naming containing **increment** rather than click in API route designs.
- Must run locally on SQL Server LocalDB: `(localdb)\MSSQLLocalDB`
- Must run in production on Azure SQL.
- Write operations must be atomic.
- Prefer small, cohesive PRs mapped to roadmap milestones.

## Branching and PR hygiene

- One branch per milestone: `milestone/M{n}-{slug}`
- Keep PRs small; each PR should map to:
  - one epic, or
  - a cohesive subset of tasks that remains reviewable in one sitting
- Commit convention: `M{n}: <area> - <action>`
  - Examples:
    - `M1: infra - add DbContext and migrations`
    - `M2: api - implement increment endpoint`

## Coding conventions

- Feature folders follow vertical slice structure: `Features/<Area>/<UseCase>/`
- Endpoints are thin:
  - parse + validate request
  - invoke handler
  - map response
- Application layer contains:
  - commands/queries
  - handlers
  - abstractions (interfaces) used by handlers
- Infrastructure layer contains:
  - EF Core DbContext
  - migrations
  - repository implementations

## Validation and quality gates

- Do not merge milestone work unless:
  - solution builds
  - tests pass (unit + integration where applicable)
  - endpoint contract matches DOC-04
  - docs are updated if contract/architecture changed

## References

- DOC-02 — Goals, Constraints, Non-Goals
- DOC-03 — Architecture Overview
- DOC-04 — API Contract
- DOC-05 — Persistence Design
- DOC-07 — Integration Testing Strategy
- DOC-09 — Definition of Done
