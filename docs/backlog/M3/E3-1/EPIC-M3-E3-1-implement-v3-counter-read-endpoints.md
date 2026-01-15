# EPIC: Implement v3 counter read endpoints

## Metadata
- Type: **Epic**
- Milestone: **M3**
- Epic: **E3.1**
- Iteration: **Iter 2**
- Effort: **3**
- Labels: `type:epic`, `area:api`, `area:app`, `area:infra`, `priority:p0`

## Description
Add GET /api/v3/counter (global) and GET /api/v3/counter/{userId} (global+user). Global and user counters are derived directly from the event store (`write.Events`) to ensure consistency. AC: matches DOC-04. Refs: DOC-04, DOC-05.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
