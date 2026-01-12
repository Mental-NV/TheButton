# EPIC: Agent enablement (scripts, tools, explicit manual steps)

## Metadata
- Type: **Epic**
- Milestone: **M0**
- Epic: **E0.2**
- Iteration: **Iter 1**
- Effort: **3**
- Labels: `type:epic`, `area:ops`, `area:testing`, `priority:p0`

## Description
Add repo tooling and scripts to maximize AI-agent autonomy; explicitly surface manual steps the agent must request from user. Refs: DOC-01,DOC-06,DOC-07.

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per `docs/MANUAL-STEPS.md`

## Links
- Roadmap: `docs/roadmap.md`
- Agent instructions: `docs/DOC-01-agent-instructions.md`
- Manual steps: `docs/MANUAL-STEPS.md`
