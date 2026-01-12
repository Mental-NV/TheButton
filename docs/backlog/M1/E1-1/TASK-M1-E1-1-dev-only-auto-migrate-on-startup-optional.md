# Dev-only auto-migrate on startup (optional)

## Metadata
- Type: **Task**
- Milestone: **M1**
- Epic: **E1.1**
- Iteration: **Iter 2**
- Effort: **2**
- Labels: `type:task`, `area:api`, `priority:p1`

## Description
Apply migrations at startup only in Development (guarded). AC: fresh clone runs with minimal steps. Refs: DOC-06.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
