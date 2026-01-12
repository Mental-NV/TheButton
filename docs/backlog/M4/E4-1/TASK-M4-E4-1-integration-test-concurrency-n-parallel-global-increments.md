# Integration test: concurrency (N parallel global increments)

## Metadata
- Type: **Task**
- Milestone: **M4**
- Epic: **E4.1**
- Iteration: **Iter 4**
- Effort: **3**
- Labels: `type:task`, `area:testing`, `priority:p1`, `testing`

## Description
Run N parallel POST /api/v3/counter; assert events==N; globalValue reaches expected max; no user projection changes. Refs: DOC-05,DOC-07.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
