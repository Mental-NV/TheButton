# DOC-00 — Index

This folder contains the authoritative planning and implementation guidance for TheButton.API refactor.

## Documents

- **DOC-01 Agent Operating Instructions** — `DOC-01-agent-instructions.md`
- **DOC-02 Product Goals, Constraints, Non-Goals** — `DOC-02-goals-constraints-non-goals.md`
- **DOC-03 Architecture Overview** — `DOC-03-architecture-overview.md`
- **DOC-04 API Contract** — `DOC-04-api-contract.md`
- **DOC-05 Persistence Design** — `DOC-05-persistence-design.md`
- **DOC-06 Local Development & Migrations** — `DOC-06-localdev-and-migrations.md`
- **DOC-07 Integration Testing Strategy** — `DOC-07-integration-testing.md`
- **DOC-08 Observability & Reliability** — `DOC-08-observability-and-reliability.md`
- **DOC-09 Definition of Done** — `DOC-09-definition-of-done.md`

## How to use

- Start with **DOC-01** and **DOC-02**.
- Use **DOC-03** as the reference for layering, folder structure, and CQRS style (no MediatR).
- Use **DOC-04** and **DOC-05** as the source of truth for contract + data model.
- Use **DOC-06**/**DOC-07** to implement and verify local and CI behavior.
- Use **DOC-08** for operational hardening.
- Use **DOC-09** for milestone acceptance criteria.
