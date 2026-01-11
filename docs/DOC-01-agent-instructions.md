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
- Prefer small, cohesive PRs mapped to roadmap milestones and epics.

## Branching and PR hygiene (updated)

### Branching model

- **One branch per PR.** Create a fresh branch for each PR and delete it after merge.
- Branch naming convention:

  - `pr/M{n}-{area}-{slug}`

  Examples:
  - `pr/M1-infra-initial-migration`
  - `pr/M2-api-increment-endpoint`
  - `pr/M4-testing-localdb-factory`

### Merge strategy

- PRs merge into `main` **as soon as**:
  - the PR scope (task/epic slice) is complete, and
  - CI is green, and
  - the PR meets the acceptance criteria for its backlog item(s).

- Avoid long-lived integration branches. Milestone progress is tracked via:
  - GitHub **Milestone** field (e.g., M2),
  - **Epic** field (e.g., E2.1),
  - labels (e.g., `area:infra`, `priority:p0`),
  not by keeping a milestone branch open.

### PR sizing rules

- Keep PRs small; each PR should map to:
  - **one epic**, or
  - a **cohesive subset of tasks** that remains reviewable in one sitting.
- If a PR mixes multiple concerns (e.g., infra + endpoint + tests), split it.

### Keeping branches current

- Always branch from latest `main`.
- If a PR lives more than a short time, **rebase onto `main`** (preferred) to keep history simple and reduce merge conflicts.
- Do not merge `main` into the PR branch unless rebasing is blocked by policy.

### Merge method recommendation

- Prefer **Squash merge** so `main` remains linear and each PR produces one clean commit.
  - In this mode, the **PR title** becomes the commit message and must follow the commit convention below.

## Commit convention

Use:

- `M{n}: <area> - <action>`

Examples:
- `M1: infra - add DbContext and migrations`
- `M2: api - implement increment endpoint`

Guidelines:
- `<area>` should be consistent and limited to: `api`, `app`, `domain`, `infra`, `testing`, `docs`, `ops`, `solution`.
- If using squash merges, enforce this convention in **PR titles**.
- If not squashing, enforce this convention in **commit messages** (or at minimum on the final merge commit).

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

Do not merge unless:

- solution builds
- tests pass (unit + integration where applicable)
- endpoint contract matches DOC-04 (when endpoints are touched)
- docs are updated if contract/architecture changed

## References

- DOC-02 — Goals, Constraints, Non-Goals
- DOC-03 — Architecture Overview
- DOC-04 — API Contract
- DOC-05 — Persistence Design
- DOC-07 — Integration Testing Strategy
- DOC-09 — Definition of Done
