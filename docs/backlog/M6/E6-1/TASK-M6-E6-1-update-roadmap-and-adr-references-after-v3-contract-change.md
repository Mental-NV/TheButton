# Update roadmap and ADR references after v3 contract change

## Metadata
- Type: **Task**
- Milestone: **M6**
- Epic: **E6.1**
- Iteration: **Iter 4**
- Effort: **2**
- Labels: `type:task`, `area:docs`, `priority:p1`, `docs`

## Description
Ensure roadmap and ADRs refer to v3 routes; add note that UserId is optional in event payload and idempotency is scoped by operation. AC: docs consistent. Refs: roadmap.md,ADR-002,ADR-003.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
