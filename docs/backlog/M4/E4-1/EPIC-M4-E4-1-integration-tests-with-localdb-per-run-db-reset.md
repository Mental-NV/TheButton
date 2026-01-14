# EPIC: Integration tests with LocalDB (per-run DB + reset)

## Metadata
- Type: **Epic**
- Milestone: **M4**
- Epic: **E4.1**
- Iteration: **Iter 3**
- Effort: **5**
- Labels: `type:epic`, `area:testing`, `priority:p0`, `testing`

## Description
Implement DOC-07 harness with  - [Integration test: POST /counter (global) increments and persists event](TASK-M4-E4-1-integration-test-post-counter-global-increments-and-persists-event.md)
  - [Integration test: POST /counter?userId={guid} increments user and persists event](TASK-M4-E4-1-integration-test-post-counter-userid-increments-user-and-persists-event.md)
  - [Provision per-run unique LocalDB database and apply migrations](TASK-M4-E4-1-provision-per-run-unique-localdb-database-and-apply-migrations.md)

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
