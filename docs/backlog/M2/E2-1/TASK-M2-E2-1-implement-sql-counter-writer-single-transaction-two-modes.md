# Implement SQL counter writer (single-transaction, two modes)

## Metadata
- Type: **Task**
- Milestone: **M2**
- Epic: **E2.1**
- Iteration: **Iter 2**
- Effort: **4**
- Labels: `type:task`, `area:infra`, `priority:p0`

## Description
Repository executes atomic transaction; mode A: GlobalIncrement inserts event (UserId null) and stores result; mode B: UserIncrement upserts/increments read.UserCounters, inserts event with UserId+UserVersion, stores result. AC: correct under retries. Refs: DOC-05.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
