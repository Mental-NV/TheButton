# Define Application abstractions for counter writer and reads

## Metadata
- Type: **Task**
- Milestone: **M2**
- Epic: **E2.1**
- Iteration: **Iter 2**
- Effort: **2**
- Labels: `type:task`, `area:app`, `priority:p0`

## Description
Add interfaces for write and read paths used by handlers; no EF Core references in Application. AC: Application compiles standalone. Refs: DOC-03.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
