# Create integration test project and test server factory

## Metadata
- Type: **Task**
- Milestone: **M4**
- Epic: **E4.1**
- Iteration: **Iter 3**
- Effort: **2**
- Labels: `type:task`, `area:testing`, `priority:p0`, `testing`

## Description
Add test project; WebApplicationFactory overrides config to use per-run connection string; uses v3 routes. AC: can call API endpoints. Refs: DOC-07,DOC-04.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
