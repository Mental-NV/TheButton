# Update DOC-05 to reflect optional userId + idempotency scoping

## Metadata
- Type: **Task**
- Milestone: **M1**
- Epic: **E1.1**
- Iteration: **Iter 2**
- Effort: **2**
- Labels: `type:task`, `area:docs`, `priority:p0`, `docs`

## Description
Amend persistence doc to specify optional UserId/UserVersion on events; filtered unique index; Commands uniqueness key includes Operation and optional UserId. AC: docs match implementation. Refs: DOC-05,DOC-04.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
