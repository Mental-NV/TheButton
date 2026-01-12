# Add Minimal API endpoint POST /api/v3/counter/{userId}

## Metadata
- Type: **Task**
- Milestone: **M2**
- Epic: **E2.1**
- Iteration: **Iter 2**
- Effort: **2**
- Labels: `type:task`, `area:api`, `priority:p0`

## Description
Implement endpoint with route userId validation + Idempotency-Key; uses v3 group. AC: matches DOC-04 exactly. Refs: DOC-04.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
