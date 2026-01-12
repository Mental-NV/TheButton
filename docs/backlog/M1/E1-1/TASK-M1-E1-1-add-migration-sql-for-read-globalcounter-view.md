# Add migration SQL for read.GlobalCounter view

## Metadata
- Type: **Task**
- Milestone: **M1**
- Epic: **E1.1**
- Iteration: **Iter 2**
- Effort: **2**
- Labels: `type:task`, `area:infra`, `priority:p0`

## Description
Create view returning MAX(Position) for EventType='CounterIncremented'. AC: view exists after migration. Refs: DOC-05.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
