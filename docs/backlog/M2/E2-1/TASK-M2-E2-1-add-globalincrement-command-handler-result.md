# Add GlobalIncrement command/handler/result

## Metadata
- Type: **Task**
- Milestone: **M2**
- Epic: **E2.1**
- Iteration: **Iter 2**
- Effort: **2**
- Labels: `type:task`, `area:app`, `priority:p0`

## Description
Implement command/handler for POST /counter; result returns globalValue only. Uses Operation=GlobalIncrement in idempotency. AC: handler unit-testable. Refs: DOC-04,DOC-05.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
