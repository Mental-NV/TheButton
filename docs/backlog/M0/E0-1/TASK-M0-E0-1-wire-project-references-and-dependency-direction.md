# Wire project references and dependency direction

## Metadata
- Type: **Task**
- Milestone: **M0**
- Epic: **E0.1**
- Iteration: **Iter 1**
- Effort: **2**
- Labels: `type:task`, `area:solution`, `priority:p0`

## Description
Enforce: Application->Domain; Infrastructure->(Application,Domain); Api->Application (+ Infrastructure via composition only). AC: no forbidden references. Refs: DOC-03.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
