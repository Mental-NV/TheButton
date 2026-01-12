# EPIC: Restore V2 endpoints + tests (backward compatibility guardrail)

## Metadata
- Type: **Epic**
- Milestone: **M0**
- Epic: **E0.3**
- Iteration: **Iter 1**
- Effort: **3**
- Labels: `type:epic`, `area:api`, `area:testing`, `priority:p0`, `backward-compat`

## Description
Ensure V2 endpoints remain available and behavior-compatible while V3 is introduced. Restore any removed V2 integration tests and lock in the non-regression policy for V2.

References:
- docs/DOC-01-agent-instructions.md (Backward compatibility policy)
- docs/DOC-03-architecture-overview.md (V2/V3 parallel structure)

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
