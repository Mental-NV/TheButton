# EPIC: Implement v3 counter write endpoints (atomic + idempotent)

## Metadata
- Type: **Epic**
- Milestone: **M2**
- Epic: **E2.1**
- Iteration: **Iter 2**
- Effort: **5**
- Labels: `type:epic`, `area:api`, `area:app`, `area:infra`, `priority:p0`

## Description
Implement POST /api/v3/counter (global) and POST /api/v3/counter/{userId} (global+user) per DOC-04 using Variant A transactional projections. AC: atomic write; idempotency enforced; correct responses. Refs: DOC-04,DOC-05,DOC-09.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
