# EPIC: Add EF Core persistence with write/read schemas and migrations

## Metadata
- Type: **Epic**
- Milestone: **M1**
- Epic: **E1.1**
- Iteration: **Iter 1**
- Effort: **5**
- Labels: `type:epic`, `area:infra`, `priority:p0`

## Description
Implement [DOC-05 Persistence Design](../../../DOC-05-persistence-design.md) schema via EF Core migrations; LocalDB + Azure SQL config per [DOC-06 Local Development & Migrations](../../../DOC-06-localdev-and-migrations.md). Includes optional userId event semantics and idempotency scoping. AC: migrations apply; DB created locally. Refs: [DOC-05 Persistence Design](../../../DOC-05-persistence-design.md),[DOC-06 Local Development & Migrations](../../../DOC-06-localdev-and-migrations.md),[DOC-09 Definition of Done](../../../DOC-09-definition-of-done.md).

## Acceptance criteria
- Implemented as described above
- Local verification steps are documented in the PR body (`.pr-body.md`)
- CI is green (agent uses `gh pr checks --watch`)
- Manual steps (if any) are explicitly requested per [Manual Steps Checklist](../../../MANUAL-STEPS.md)

## Links
- Roadmap: [Roadmap](../../../roadmap.md)
- Agent instructions: [DOC-01 Agent Operating Instructions](../../../DOC-01-agent-instructions.md)
- Manual steps: [Manual Steps Checklist](../../../MANUAL-STEPS.md)
