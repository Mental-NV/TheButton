# Add health check endpoints (live/ready)

## Metadata
- Type: **Task**
- Milestone: **M5**
- Epic: **E5.1**
- Iteration: **Iter 3**
- Effort: **2**
- Labels: `type:task`, `area:ops`, `priority:p1`

## Description
Implement live probe and DB-ready probe. AC: ready fails if DB unreachable. Refs: DOC-08.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
