# Add EF Core SQL Server packages and DbContext skeleton

## Metadata
- Type: **Task**
- Milestone: **M1**
- Epic: **E1.1**
- Iteration: **Iter 1**
- Effort: **2**
- Labels: `type:task`, `area:infra`, `priority:p0`

## Description
Add EF Core provider; create TheButtonDbContext with write/read schemas. AC: Api resolves DbContext. Refs: DOC-05.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
