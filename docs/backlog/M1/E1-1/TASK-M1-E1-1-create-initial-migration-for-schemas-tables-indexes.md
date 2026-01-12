# Create initial migration for schemas/tables/indexes

## Metadata
- Type: **Task**
- Milestone: **M1**
- Epic: **E1.1**
- Iteration: **Iter 2**
- Effort: **3**
- Labels: `type:task`, `area:infra`, `priority:p0`

## Description
Add migration creating write/read schemas, tables, indexes, constraints (including filtered index). AC: dotnet ef database update works on LocalDB. NOTE: agent must ask user to run migration command if generation is required. Refs: DOC-06,MANUAL-STEPS.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
