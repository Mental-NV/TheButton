# Add dotnet tool manifest with dotnet-ef

## Metadata
- Type: **Task**
- Milestone: **M0**
- Epic: **E0.2**
- Iteration: **Iter 1**
- Effort: **2**
- Labels: `type:task`, `area:infra`, `priority:p0`

## Description
Add .config/dotnet-tools.json and pin dotnet-ef; add instructions to run: dotnet tool restore. AC: dotnet tool restore works. Refs: DOC-06.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
