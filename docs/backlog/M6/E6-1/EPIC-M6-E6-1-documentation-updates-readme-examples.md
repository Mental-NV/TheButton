# EPIC: Documentation updates (README + examples)

## Metadata
- Type: **Epic**
- Milestone: **M6**
- Epic: **E6.1**
- Iteration: **Iter 4**
- Effort: **3**
- Labels: `type:epic`, `area:docs`, `priority:p1`, `docs`

## Description
Update README and docs to reflect v3 endpoints, LocalDB workflow, idempotency usage, and manual steps. AC: new dev can run from scratch. Refs: DOC-04,DOC-06,MANUAL-STEPS.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
