# CI: ensure integration tests run on Windows with LocalDB

## Metadata
- Type: **Task**
- Milestone: **M4**
- Epic: **E4.1**
- Iteration: **Iter 4**
- Effort: **3**
- Labels: `type:task`, `area:ops`, `priority:p0`, `testing`

## Description
Update CI workflow to run integration tests on windows-latest; use scripts/test.*; ensure LocalDB connectivity. NOTE: agent must ask user if CI file is incomplete or needs secrets. Refs: DOC-07,MANUAL-STEPS.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
