# Add bounded retry in SQL counter writer

## Metadata
- Type: **Task**
- Milestone: **M5**
- Epic: **E5.2**
- Iteration: **Iter 3**
- Effort: **2**
- Labels: `type:task`, `area:infra`, `priority:p1`

## Description
Retry 2-3 times on known transient/concurrency exceptions; log attempt count; fail with 409 when exhausted. AC: no infinite loops; deterministic failure. Refs: DOC-08.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
