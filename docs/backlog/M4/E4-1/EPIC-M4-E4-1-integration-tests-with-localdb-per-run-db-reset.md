# EPIC: Integration tests with LocalDB (per-run DB + reset)

## Metadata
- Type: **Epic**
- Milestone: **M4**
- Epic: **E4.1**
- Iteration: **Iter 3**
- Effort: **5**
- Labels: `type:epic`, `area:testing`, `priority:p0`, `testing`

## Description
Implement DOC-07 harness with WebApplicationFactory and LocalDB; create per-run DB; reset between tests. AC: runs locally and in CI. Refs: DOC-07,DOC-09.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
