# Map entities for write.Events/write.Commands/read.UserCounters

## Metadata
- Type: **Task**
- Milestone: **M1**
- Epic: **E1.1**
- Iteration: **Iter 1**
- Effort: **4**
- Labels: `type:task`, `area:infra`, `priority:p0`

## Description
Create entities + configurations: schemas, keys, indexes. Ensure Events support optional UserId/UserVersion and Commands support Operation+UserId scoping. AC: model matches DOC-05+Deltas. Refs: DOC-05,DOC-04.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
