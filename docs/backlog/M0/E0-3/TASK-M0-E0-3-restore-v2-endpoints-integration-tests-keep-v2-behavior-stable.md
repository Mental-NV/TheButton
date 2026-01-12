# Restore V2 endpoints + integration tests; keep V2 behavior stable

## Metadata
- Type: **Task**
- Milestone: **M0**
- Epic: **E0.3**
- Iteration: **Iter 1**
- Effort: **5**
- Labels: `type:task`, `area:api`, `area:testing`, `priority:p0`, `backward-compat`, `risk`

## Description
Restore V2 endpoints and their integration tests (do not remove, do not change behavior). The agent may implement V2 endpoints using Minimal APIs for unification, but must preserve the V2 routes, request/response shapes, and observable behavior.

Implementation notes:
- V2 and V3 must run side-by-side via Asp.Versioning route groups.
- Keep V2-specific DTOs/models isolated from V3 to prevent accidental breaking changes.
- Restore integration tests from git history if removed (e.g., locate historical version of `tests/TheButton.Api.IntegrationTests/CounterApiTests.cs`).
- Ensure CI runs V2 integration tests.

Checkpoint / risk:
- This is a **manual review checkpoint**: request user review before moving to M1 work.

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
