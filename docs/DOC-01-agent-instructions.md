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
- Use naming containing **increment** rather than click in API designs (internal naming); external API contract is defined in [DOC-04 API Contract](DOC-04-api-contract.md).
- Must run locally on SQL Server LocalDB: `(localdb)\MSSQLLocalDB`
- Must run in production on Azure SQL.
- Write operations must be atomic.
- Prefer small, cohesive PRs mapped to roadmap milestones and epics.

## Manual steps protocol

When a manual step is required (e.g., running EF migrations, provisioning Azure resources), the agent must:

1. **Stop** at an appropriate task boundary
2. **Explicitly tell the user** what command/action to run
3. **Wait for user confirmation** before continuing

See [Manual Steps Checklist](MANUAL-STEPS.md) for the complete checklist of manual steps and expected human actions.

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

## Required PR workflow (Windows / PowerShell)

For each PR:

1. **Plan first** (do not code yet)
   - Identify the backlog item(s) targeted.
   - List files to change/create.
   - List commands to run.
   - List acceptance criteria and how they will be verified.
   - Identify any likely manual steps (if any).

2. **Update Roadmap Status (Work in progress)**
   - Update the status of the targeted item(s) in [Roadmap](roadmap.md) to `**[InProgress]**`.
   - If this is the first item in an Epic, update the Epic status to `**[InProgress]**`.

3. **Create branch**
   - `git checkout main`
   - `git pull --ff-only`
   - `git switch -c pr/M{n}-{area}-{slug}`

4. **Implement**
   - Make focused changes only for the PR scope.

5. **Verify locally**
   - `dotnet build`
   - `dotnet test` (or `.\scripts\test.ps1` if present)

6. **Commit and push**
   - `git add -A`
   - `git commit -m "M{n}: <area> - <action>"`
   - `git push -u origin HEAD`

7. **Update Roadmap Status (In Review)**
   - Update the status of the targeted item(s) in [Roadmap](roadmap.md) to `**[InReview]**`.

8. **Create PR**
   - Create `.pr-body.md` (required; template below).
   - `gh pr create --title "M{n}: <area> - <action>" --body-file .\.pr-body.md`

9. **Monitor CI and pull logs on failure**
   - `gh pr checks --watch`
   - If failing:
     - `gh run list --branch <branch> --limit 5`
     - `gh run view <run-id> --log-failed`
    - Update status to `**[InProgress]**` if major fixes are needed.
    - Fix, commit, push until green.

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

For every PR, create `.pr-body.md` with the following structure:

```md
## Backlog item(s)
- <title> (Milestone: M{n}, Epic: E{n.n})

## Scope
- <what changed, bullet points>

## Verification
- `dotnet build`
- `dotnet test` (or `.\scripts\test.ps1`)

## Notes / Risks
- <anything that reviewers should know>

## Manual steps required
- None
```

If manual steps are required, replace the last section with:

```md
## Manual steps required
1. <exact command(s) the user must run>
2. <what output to confirm / what success looks like>
```

## Manual steps rule

If a PR requires:
- generating EF Core migrations, or
- provisioning Azure resources/secrets, or
- CI environment changes that require repo settings,

the agent must:
- stop at an appropriate boundary,
- provide exact commands/actions from [Manual Steps Checklist](MANUAL-STEPS.md),
- ask the user to confirm success before proceeding.

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

Do not open a PR (or mark it ready) unless:

- solution builds
- tests pass (unit + integration where applicable)
- endpoint contract matches [DOC-04 API Contract](DOC-04-api-contract.md) (when endpoints are touched)
- docs are updated if contract/architecture changed

## References

- [DOC-02 Product Goals, Constraints, Non-Goals](DOC-02-goals-constraints-non-goals.md)
- [DOC-03 Architecture Overview](DOC-03-architecture-overview.md)
- [DOC-04 API Contract](DOC-04-api-contract.md)
- [DOC-05 Persistence Design](DOC-05-persistence-design.md)
- [DOC-07 Integration Testing Strategy](DOC-07-integration-testing.md)
- [DOC-08 Observability & Reliability](DOC-08-observability-and-reliability.md)
- [DOC-09 Definition of Done](DOC-09-definition-of-done.md)
- [Manual Steps Checklist](MANUAL-STEPS.md)
