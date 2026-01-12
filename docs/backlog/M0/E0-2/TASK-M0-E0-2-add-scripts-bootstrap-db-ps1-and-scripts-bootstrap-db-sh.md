# Add scripts/bootstrap-db.ps1 and scripts/bootstrap-db.sh

## Metadata
- Type: **Task**
- Milestone: **M0**
- Epic: **E0.2**
- Iteration: **Iter 2**
- Effort: **2**
- Labels: `type:task`, `area:ops`, `priority:p0`

## Description
Scripts apply migrations to LocalDB (and optionally a supplied connection string). AC: fresh clone can create DB with one command. Refs: DOC-06.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
