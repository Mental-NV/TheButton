# Provision per-run unique LocalDB database and apply migrations

## Metadata
- Type: **Task**
- Milestone: **M4**
- Epic: **E4.1**
- Iteration: **Iter 3**
- Effort: **3**
- Labels: `type:task`, `area:testing`, `priority:p0`, `testing`

## Description
Create DB name per run; apply migrations once. NOTE: if migrations need generation, agent must ask user to run dotnet ef migrations add. AC: no manual DB creation. Refs: DOC-06,MANUAL-STEPS.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
