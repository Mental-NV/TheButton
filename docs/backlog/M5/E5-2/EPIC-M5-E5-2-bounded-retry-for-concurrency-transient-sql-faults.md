# EPIC: Bounded retry for concurrency/transient SQL faults

## Metadata
- Type: **Epic**
- Milestone: **M5**
- Epic: **E5.2**
- Iteration: **Iter 3**
- Effort: **3**
- Labels: `type:epic`, `area:infra`, `priority:p1`

## Description
Add bounded retry for write transaction on transient/concurrency errors; keep idempotency correct. AC: stable under stress. Refs: DOC-08,DOC-05.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
