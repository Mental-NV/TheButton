# Define idempotency schema scoped by operation and user

## Metadata
- Type: **Task**
- Milestone: **M1**
- Epic: **E1.1**
- Iteration: **Iter 1**
- Effort: **3**
- Labels: `type:task`, `area:infra`, `priority:p0`

## Description
Implement write.Commands uniqueness as (Operation, UserId, IdempotencyKey); UserId nullable; Operation required (GlobalIncrement/UserIncrement). AC: same Idempotency-Key across operations cannot collide. Refs: DOC-05,DOC-04.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
