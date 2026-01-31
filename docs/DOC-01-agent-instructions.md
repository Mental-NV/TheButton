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
- Use naming containing **increment** rather than click in API designs (internal naming); external API contract is defined in [DOC-04 API Contract](/docs/DOC-04-api-contract.md).
- Must run locally on SQL Server LocalDB: `(localdb)\MSSQLLocalDB`
- Must run in production on Azure SQL.
- Write operations must be atomic.
- Prefer small, cohesive PRs mapped to roadmap milestones and epics.

## Manual steps protocol

When a manual step is required (e.g., running EF migrations, provisioning Azure resources), the agent must:

1. **Stop** at an appropriate task boundary
2. **Explicitly tell the user** what command/action to run
3. **Wait for user confirmation** before continuing

See [Manual Steps Checklist](/docs/MANUAL-STEPS.md) for the complete checklist of manual steps and expected human actions.

## Branching and PR hygiene

### Branching model

- **One branch per PR.** Create a fresh branch for each PR and delete it after merge.
- Branch naming convention:
  - `pr/M{n}-{area}-{slug}`

Examples:
- `pr/M1-infra-initial-migration`
- `pr/M2-api-v3-counter-endpoints`
- `pr/M4-testing-localdb-factory`

### Merge responsibility

- The agent **must not** merge or close PRs.
- The user performs the final merge and PR closure after review and green CI.

### PR sizing rules

- Keep PRs small; each PR should map to:
  - **one epic**, or
  - a **cohesive subset of tasks** that remains reviewable in one sitting.
- If a PR mixes multiple concerns (e.g., infra + endpoint + tests), split it.

### Keeping branches current

- Always branch from latest `main`:
  - `git checkout main`
  - `git pull --ff-only`
- If a PR lives more than a short time, **rebase onto `main`** (preferred) to reduce merge conflicts.
- Do not merge `main` into the PR branch unless rebasing is blocked by policy.

### Merge method recommendation

- Prefer **Squash merge** so `main` remains linear and each PR produces one clean commit.
  - In this mode, the PR title becomes the commit message and must follow the commit convention below.

## Commit convention

Use:

- `M{n}: <area> - <action>`

Examples:
- `M1: infra - add DbContext and migrations`
- `M2: api - implement v3 counter endpoints`

Guidelines:
- `<area>` should be consistent and limited to: `api`, `app`, `domain`, `infra`, `testing`, `docs`, `ops`, `solution`.
- If using squash merges, enforce this convention in **PR titles**.
- If not squashing, enforce this convention in **commit messages** (or at minimum on the final merge commit).

## Required PR workflow

For each PR, follow the automated **PR Skill** and **Workflow** defined in `.agent/`.

1. **Verify Policies**: Ensure the branch name follows `pr/M{n}-{area}-{slug}` and the PR title matches `M{n}: <area> - <action>`.
2. **Execute**: Use the `/pr` workflow (slash command) or follow the instructions in `.agent/skills/pr/SKILL.md`.
3. **Template**: Always use the `.pr-body.md` template defined below.
4. **Monitor**: Use `gh pr checks --watch` to monitor CI status.


## Backlog Status Management

The agent is responsible for maintaining the visual status of the backlog in `docs/roadmap.md` following these rules:

### Status transition rules (Work Items)
- **Start working**: Update item to `**[InProgress]**`.
- **Finish working (PR opened)**: Update item to `**[InReview]**`.
- **Fixes required during review**: Update item back to `**[InProgress]**`.
- **Review successful (Merged)**: Update item to `**[Done]**`.

### Status transition rules (Epics)
- **InProgress**: When any child work item starts, the parent Epic must be marked as `**[InProgress]**`.
- **Done**: When **all** child work items under an Epic are `**[Done]**`, the parent Epic must be marked as `**[Done]**`.

## PR body template (required)

For every PR, generate `/.pr-body.md` using the template located at:
`[PR Body Template](/.agent/skills/pr/resources/pr-body-template.md)`.


## Manual steps rule

If a PR requires:
- generating EF Core migrations, or
- provisioning Azure resources/secrets, or
- CI environment changes that require repo settings,

the agent must:
- stop at an appropriate boundary,
- provide exact commands/actions from [Manual Steps Checklist](/docs/MANUAL-STEPS.md),
- ask the user to confirm success before proceeding.

## Validation and quality gates

Do not open a PR (or mark it ready) unless:

- solution builds
- tests pass (unit + integration where applicable)
- endpoint contract matches [DOC-04 API Contract](/docs/DOC-04-api-contract.md) (when endpoints are touched)
- docs are updated if contract/architecture changed
- code quality gates in [DOC-09 Definition of Done](/docs/DOC-09-definition-of-done.md) are satisfied

## References

- [DOC-02 Product Goals, Constraints, Non-Goals](/docs/DOC-02-goals-constraints-non-goals.md)
- [DOC-03 Architecture Overview](/docs/DOC-03-architecture-overview.md)
- [DOC-04 API Contract](/docs/DOC-04-api-contract.md)
- [DOC-05 Persistence Design](/docs/DOC-05-persistence-design.md)
- [DOC-07 Integration Testing Strategy](/docs/DOC-07-integration-testing.md)
- [DOC-08 Observability & Reliability](/docs/DOC-08-observability-and-reliability.md)
- [DOC-09 Definition of Done](/docs/DOC-09-definition-of-done.md)
- [Manual Steps Checklist](/docs/MANUAL-STEPS.md)
