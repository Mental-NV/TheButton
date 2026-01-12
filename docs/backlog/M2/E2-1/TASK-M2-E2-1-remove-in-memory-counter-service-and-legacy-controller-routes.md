# Remove in-memory counter service and legacy controller routes

## Metadata
- Type: **Task**
- Milestone: **M2**
- Epic: **E2.1**
- Iteration: **Iter 2**
- Effort: **2**
- Labels: `type:task`, `area:api`, `priority:p0`, `breaking-change`

## Description
Eliminate old CounterService/controller to ensure production uses DB-backed v3 endpoints. AC: no in-memory increment remains; old routes removed or redirected. Refs: DOC-02,DOC-09.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
