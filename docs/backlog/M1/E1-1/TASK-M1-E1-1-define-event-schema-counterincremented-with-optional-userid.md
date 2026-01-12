# Define event schema: CounterIncremented with optional UserId

## Metadata
- Type: **Task**
- Milestone: **M1**
- Epic: **E1.1**
- Iteration: **Iter 1**
- Effort: **3**
- Labels: `type:task`, `area:infra`, `priority:p0`

## Description
Implement write.Events columns for UserId (nullable) and UserVersion (nullable); create filtered unique index on (UserId,UserVersion) WHERE UserId IS NOT NULL. AC: parallel user increments preserve uniqueness; global-only increments allowed. Refs: DOC-05,DOC-04.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
