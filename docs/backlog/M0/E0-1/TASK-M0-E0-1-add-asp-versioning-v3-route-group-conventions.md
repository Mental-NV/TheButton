# Add Asp.Versioning v3 route group conventions

## Metadata
- Type: **Task**
- Milestone: **M0**
- Epic: **E0.1**
- Iteration: **Iter 1**
- Effort: **2**
- Labels: `type:task`, `area:api`, `priority:p0`

## Description
Keep Asp.Versioning; map v3 routes via versioned route groups (URL segment) so base path is /api/v3. AC: group routing produces /api/v3 and ApiExplorer shows v3. Refs: DOC-04.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
