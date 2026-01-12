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
- Use naming containing **increment** rather than click in API designs (internal naming); external API contract is defined in DOC-04.
- Must run locally on SQL Server LocalDB: `(localdb)\MSSQLLocalDB`
- Must run in production on Azure SQL.
- Write operations must be atomic.
- Prefer small, cohesive PRs mapped to roadmap milestones and epics.
- If a manual step is required, follow `docs/MANUAL-STEPS.md` and explicitly prompt the user.

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

2. **Create branch**
   - `git checkout main`
   - `git pull --ff-only`
   - `git switch -c pr/M{n}-{area}-{slug}`

3. **Implement**
   - Make focused changes only for the PR scope.

4. **Verify locally**
   - `dotnet build`
   - `dotnet test` (or `.\scripts\test.ps1` if present)

5. **Commit and push**
   - `git add -A`
   - `git commit -m "M{n}: <area> - <action>"`
   - `git push -u origin HEAD`

6. **Create PR**
   - Create `.pr-body.md` (required; template below).
   - `gh pr create --title "M{n}: <area> - <action>" --body-file .\.pr-body.md`

7. **Monitor CI and pull logs on failure**
   - `gh pr checks --watch`
   - If failing:
     - `gh run list --branch <branch> --limit 5`
     - `gh run view <run-id> --log-failed`
   - Fix, commit, push until green.

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
- provide exact commands/actions from `docs/MANUAL-STEPS.md`,
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
- endpoint contract matches DOC-04 (when endpoints are touched)
- docs are updated if contract/architecture changed

## References

- DOC-02 — Goals, Constraints, Non-Goals
- DOC-03 — Architecture Overview
- DOC-04 — API Contract
- DOC-05 — Persistence Design
- DOC-07 — Integration Testing Strategy
- DOC-08 — Observability & Reliability
- DOC-09 — Definition of Done
- MANUAL-STEPS — Manual Steps Checklist
