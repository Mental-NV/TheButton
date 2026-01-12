# EPIC: CI split (backend/web vs mobile) to avoid MAUI workloads on backend changes

## Metadata
- Type: **Epic**
- Milestone: **M1**
- Epic: **E1.2**
- Iteration: **Iter 2**
- Effort: **3**
- Labels: `type:epic`, `area:ops`, `priority:p0`, `risk`

## Description
Speed up CI by avoiding MAUI workload install/build/test when only backend/web changes occur.

Context:
- Current slowest part: MAUI workloads and mobile build/tests.
- Mobile areas:
  - `.github/workflows/e2e-mobile.yml`
  - `.maestro/**`
  - `scripts/run-android-e2e.sh`
  - `scripts/run-ios-e2e.sh`
  - `src/TheButton.Mobile.Core/**`
  - `src/TheButton.Mobile.Infrastructure/**`
  - `src/TheButton.Mobile/**`
  - `tools/TheButton.MockApi/**`

Goal:
- Separate workflows and/or use path filters so backend/web CI is fast and stable, while mobile CI runs only when relevant.

Checkpoint / risk:
- This is a **risk checkpoint** (workflow behavior changes). Require user review before merging.

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
