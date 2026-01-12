# EPIC: Observability and health

## Metadata
- Type: **Epic**
- Milestone: **M5**
- Epic: **E5.1**
- Iteration: **Iter 3**
- Effort: **3**
- Labels: `type:epic`, `area:ops`, `priority:p1`

## Description
Implement /health/live and /health/ready; structured log enrichment. AC: readiness checks DB; logs include UserId/Operation/IdempotencyKey when present. Refs: DOC-08.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
