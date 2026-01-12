# Add Minimal API endpoint GET /api/v3/counter/{userId}

## Metadata
- Type: **Task**
- Milestone: **M3**
- Epic: **E3.1**
- Iteration: **Iter 2**
- Effort: **2**
- Labels: `type:task`, `area:api`, `priority:p0`

## Description
Return globalValue + userId + userValue; validate GUID. AC: matches DOC-04. Refs: DOC-04.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
