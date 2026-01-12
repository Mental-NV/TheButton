# Implement SQL read repository (global and per-user)

## Metadata
- Type: **Task**
- Milestone: **M3**
- Epic: **E3.1**
- Iteration: **Iter 2**
- Effort: **2**
- Labels: `type:task`, `area:infra`, `priority:p0`

## Description
Query read.GlobalCounter view and read.UserCounters table; return userValue=0 when missing. AC: efficient queries; no event-store scan for user reads. Refs: DOC-05.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
