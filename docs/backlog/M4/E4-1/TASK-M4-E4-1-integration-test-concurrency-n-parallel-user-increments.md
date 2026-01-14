# Integration test: concurrency (N parallel user increments)

## Metadata
- Type: **Task**
- Milestone: **M4**
- Epic: **E4.1**
- Iteration: **Iter 3**
- Effort: **3**
- Labels: `type:task`, `area:testing`, `priority:p0`, `testing`

## Description
Run N parallel POST /api/v3/counter?userId={userId}; assert userValue==N; events==N; filtered uniqueness (UserId,UserVersion) holds in write.Events. Refs: DOC-05,DOC-07.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
