# DOC-00 — Index

This folder contains the authoritative planning and implementation guidance for TheButton.API refactor.

## Documents

- [DOC-00 Index](DOC-00-index.md)
- [DOC-01 Agent Operating Instructions](DOC-01-agent-instructions.md)
- [DOC-02 Product Goals, Constraints, Non-Goals](DOC-02-goals-constraints-non-goals.md)
- [DOC-03 Architecture Overview](DOC-03-architecture-overview.md)
- [DOC-04 API Contract (v3)](DOC-04-api-contract.md)
- [DOC-05 Persistence Design](DOC-05-persistence-design.md)
- [DOC-06 Local Development & Migrations](DOC-06-localdev-and-migrations.md)
- [DOC-07 Integration Testing Strategy](DOC-07-integration-testing.md)
- [DOC-08 Observability & Reliability](DOC-08-observability-and-reliability.md)
- [DOC-09 Definition of Done](DOC-09-definition-of-done.md)

## Additional docs

- [Roadmap](roadmap.md)
- [Manual Steps Checklist](MANUAL-STEPS.md)
- **Architecture Decisions (ADRs)**
  - [ADR-001 CQRS without MediatR](ADR-001-cqrs-without-mediatr.md)
  - [ADR-002 Transactional Projections Variant A](ADR-002-transactional-projections-variant-a.md)
  - [ADR-003 Global Counter from Event Position](ADR-003-global-counter-from-event-position.md)

## How to use

- Start with [DOC-01 Agent Operating Instructions](DOC-01-agent-instructions.md) and [DOC-02 Product Goals, Constraints, Non-Goals](DOC-02-goals-constraints-non-goals.md).
- Use [DOC-03 Architecture Overview](DOC-03-architecture-overview.md) as the reference for layering, folder structure, CQRS style (no MediatR), and **Asp.Versioning v3 route groups**.
- Use [DOC-04 API Contract (v3)](DOC-04-api-contract.md) and [DOC-05 Persistence Design](DOC-05-persistence-design.md) as the source of truth for API contract + persistence model (including optional `UserId` event semantics and operation-scoped idempotency).
- Use [DOC-06 Local Development & Migrations](DOC-06-localdev-and-migrations.md)/[DOC-07 Integration Testing Strategy](DOC-07-integration-testing.md) for local dev, migrations, and integration testing strategy.
- Use [Manual Steps Checklist](MANUAL-STEPS.md) to understand when the agent must explicitly ask for human actions (e.g., generating migrations, provisioning Azure secrets).
- Use [DOC-08 Observability & Reliability](DOC-08-observability-and-reliability.md) for operational hardening.
- Use [DOC-09 Definition of Done](DOC-09-definition-of-done.md) for milestone acceptance criteria.
