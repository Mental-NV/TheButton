# EPIC: Add EF Core persistence with write/read schemas and migrations

## Metadata
- Type: **Epic**
- Milestone: **M1**
- Epic: **E1.1**
- Iteration: **Iter 1**
- Effort: **5**
- Labels: `type:epic`, `area:infra`, `priority:p0`

## Description
Implement DOC-05 schema via EF Core migrations; LocalDB + Azure SQL config per DOC-06. Includes optional userId event semantics and idempotency scoping. AC: migrations apply; DB created locally. Refs: DOC-05,DOC-06,DOC-09.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
