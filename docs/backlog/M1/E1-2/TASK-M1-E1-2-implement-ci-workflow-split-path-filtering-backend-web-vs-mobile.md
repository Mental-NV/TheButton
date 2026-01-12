# Implement CI workflow split + path filtering (backend/web vs mobile)

## Metadata
- Type: **Task**
- Milestone: **M1**
- Epic: **E1.2**
- Iteration: **Iter 2**
- Effort: **5**
- Labels: `type:task`, `area:ops`, `priority:p0`

## Description
Implement a CI strategy that avoids MAUI workloads unless mobile-related paths change.

Deliverables (recommended approach):
- Create a dedicated backend/web CI workflow (e.g., `.github/workflows/ci-backend.yml`) triggered by:
  - changes in API/web paths
  - *excluding* mobile paths
- Keep/create a dedicated mobile workflow (`e2e-mobile.yml` or `ci-mobile.yml`) triggered only when mobile paths change.
- Ensure PR checks are clear (backend CI required for most PRs; mobile CI required only when mobile changed).
- Document the workflow behavior in `docs/` (where to look, how to run, how to force-run).

Notes:
- Prefer path filters over moving folders unless restructure is clearly beneficial.
- If you propose repo restructure, do it as a separate optional follow-up task.

Verification:
- Use `gh` to verify checks on a PR that touches backend only vs one that touches mobile paths.

## Acceptance criteria
- Implemented as described above
- PR body uses `.pr-body.md` template from `docs/DOC-01-agent-instructions.md`
- Local verification is documented (build + tests)
- CI is green and agent pulled logs on failures (`gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`
- V2 backward compatibility is preserved (when applicable)

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
