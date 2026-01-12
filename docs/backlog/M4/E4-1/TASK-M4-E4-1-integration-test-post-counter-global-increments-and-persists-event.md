# Integration test: POST /counter (global) increments and persists event

## Metadata
- Type: **Task**
- Milestone: **M4**
- Epic: **E4.1**
- Iteration: **Iter 3**
- Effort: **2**
- Labels: `type:task`, `area:testing`, `priority:p0`, `testing`

## Description
Call POST /api/v3/counter; assert 1 event with UserId NULL; response globalValue equals inserted Position; Commands row scoped to GlobalIncrement. Refs: DOC-04,DOC-05,DOC-07.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
