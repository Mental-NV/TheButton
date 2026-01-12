# Add scripts/test.ps1 and scripts/test.sh

## Metadata
- Type: **Task**
- Milestone: **M0**
- Epic: **E0.2**
- Iteration: **Iter 2**
- Effort: **2**
- Labels: `type:task`, `area:testing`, `priority:p0`

## Description
Scripts run build + unit + integration tests consistently; used by CI. AC: single command runs full test suite. Refs: DOC-07.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
