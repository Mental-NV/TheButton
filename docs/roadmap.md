# Roadmap

This roadmap is designed for coding AI agents. It links milestones to epics and references the authoritative docs.

## References

- [DOC-00 Index](DOC-00-index.md)
- [DOC-01 Agent Operating Instructions](DOC-01-agent-instructions.md)
- [DOC-02 Goals, Constraints, Non-Goals](DOC-02-goals-constraints-non-goals.md)
- [DOC-03 Architecture Overview](DOC-03-architecture-overview.md)
- [DOC-04 API Contract](DOC-04-api-contract.md)
- [DOC-05 Persistence Design](DOC-05-persistence-design.md)
- [DOC-06 Local Development & Migrations](DOC-06-localdev-and-migrations.md)
- [DOC-07 Integration Testing Strategy](DOC-07-integration-testing.md)
- [DOC-08 Observability & Reliability](DOC-08-observability-and-reliability.md)
- [DOC-09 Definition of Done](DOC-09-definition-of-done.md)

## Milestones

### M0 — Planning & Solution Restructure

**Goal:** Establish solution structure and Minimal API endpoint mapping foundation aligned with Clean Architecture + Vertical Slices.

**Epics**
- **E0.1** — Introduce Clean Architecture solution structure (Api/Application/Domain/Infrastructure)

**Key outputs**
- Projects and dependency direction enforced ([DOC-03](DOC-03-architecture-overview.md))
- Minimal API mapping skeleton in place ([DOC-03](DOC-03-architecture-overview.md))
- Baseline build remains green ([DOC-09](DOC-09-definition-of-done.md))

---

### M1 — Persistence Foundations (EF Core + Migrations)

**Goal:** Implement SQL persistence layout (write/read schemas) and migrations for LocalDB + Azure SQL.

**Epics**
- **E1.1** — Add EF Core persistence with write/read schemas and migrations

**Key outputs**
- EF Core DbContext + entity mapping for event store, idempotency, projections ([DOC-05](DOC-05-persistence-design.md))
- Initial migration creates schemas/tables + view ([DOC-05](DOC-05-persistence-design.md))
- LocalDB connectivity and DB creation workflow ([DOC-06](DOC-06-localdev-and-migrations.md))

---

### M2 — Increment Write Path (Atomic + Idempotent)

**Goal:** Implement synchronous strong-consistency `POST /api/v1/counter/increment` using Variant A transactional projections.

**Epics**
- **E2.1** — Implement /counter/increment (atomic + idempotent)

**Key outputs**
- Minimal API endpoint matches contract ([DOC-04](DOC-04-api-contract.md))
- Single-transaction algorithm implemented ([DOC-05](DOC-05-persistence-design.md))
- In-memory counter removed/deprecated ([DOC-02](DOC-02-goals-constraints-non-goals.md))
- Bounded retry strategy may be deferred to M5 ([DOC-08](DOC-08-observability-and-reliability.md))

---

### M3 — Read Endpoints (Global + Per-User)

**Goal:** Provide query endpoints backed by projection tables/views.

**Epics**
- **E3.1** — Implement read endpoints (global + per-user)

**Key outputs**
- `GET /counter/global` and `GET /counter/users/{userId}` live ([DOC-04](DOC-04-api-contract.md))
- Queries read projections only ([DOC-05](DOC-05-persistence-design.md))

---

### M4 — Integration Tests (LocalDB)

**Goal:** Validate correctness under real SQL behavior: atomicity, idempotency, concurrency.

**Epics**
- **E4.1** — Integration tests with LocalDB (per-run DB + reset between tests)

**Key outputs**
- Per-run unique test DB provisioning and migrations ([DOC-07](DOC-07-integration-testing.md))
- Reset strategy for deterministic tests ([DOC-07](DOC-07-integration-testing.md))
- Core integration tests: increment, idempotency, concurrency ([DOC-07](DOC-07-integration-testing.md))

---

### M5 — Observability, Health, Hardening

**Goal:** Add operational readiness and resilience features.

**Epics**
- **E5.1** — Add health checks and structured logging
- **E5.2** — Bounded retry for concurrency/transient SQL faults

**Key outputs**
- `/health/live` and `/health/ready` endpoints ([DOC-08](DOC-08-observability-and-reliability.md))
- Structured log enrichment and correlation fields ([DOC-08](DOC-08-observability-and-reliability.md))
- Bounded retry for transient/concurrency faults ([DOC-08](DOC-08-observability-and-reliability.md))

---

### M6 — Documentation & Polish

**Goal:** Ensure repo is self-serve for developers and agents; finalize runbook-quality docs.

**Epics**
- **E6.1** — Update README with local run, migrations, API usage

**Key outputs**
- README reflects LocalDB, migrations, API usage patterns ([DOC-04](DOC-04-api-contract.md), [DOC-06](DOC-06-localdev-and-migrations.md))
- Docs updated to reflect final state ([DOC-09](DOC-09-definition-of-done.md))

## Cross-cutting conventions

- Architectural decisions are recorded as ADRs in `docs/decisions/`.
- If a decision changes an existing ADR, update that ADR and add a superseding ADR if needed.
