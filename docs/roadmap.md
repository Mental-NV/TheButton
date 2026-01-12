# Roadmap

This roadmap is designed for coding AI agents. It links milestones to epics and references the authoritative docs.

## References

- [DOC-00 Index](DOC-00-index.md)
- [DOC-01 Agent Operating Instructions](DOC-01-agent-instructions.md)
- [DOC-02 Goals, Constraints, Non-Goals](DOC-02-goals-constraints-non-goals.md)
- [DOC-03 Architecture Overview](DOC-03-architecture-overview.md)
- [DOC-04 API Contract (v3)](DOC-04-api-contract.md)
- [DOC-05 Persistence Design](DOC-05-persistence-design.md)
- [DOC-06 Local Development & Migrations](DOC-06-localdev-and-migrations.md)
- [DOC-07 Integration Testing Strategy](DOC-07-integration-testing.md)
- [DOC-08 Observability & Reliability](DOC-08-observability-and-reliability.md)
- [DOC-09 Definition of Done](DOC-09-definition-of-done.md)
- [Manual Steps Checklist](MANUAL-STEPS.md)

## Milestones

### M0 — Planning, Agent Enablement & Solution Restructure

**Goal:** Establish solution structure and endpoint mapping foundation aligned with Clean Architecture + Vertical Slices, plus tooling/scripts to maximize agent autonomy.

**Epics**
- **E0.1** — Introduce Clean Architecture solution structure (Api/Application/Domain/Infrastructure)
- **E0.2** — Agent enablement (scripts, tools, explicit manual steps)

**Key outputs**
- Projects and dependency direction enforced (DOC-03)
- Minimal API mapping skeleton in place (DOC-03)
- Asp.Versioning configured to expose **/api/v3** routes via versioned route groups (DOC-03, DOC-04)
- dotnet tools + scripts added for consistent execution (MANUAL-STEPS)
- Baseline build remains green (DOC-09)

---

### M1 — Persistence Foundations (EF Core + Migrations)

**Goal:** Implement SQL persistence layout (write/read schemas) and migrations for LocalDB + Azure SQL, including optional `UserId` event semantics and operation-scoped idempotency.

**Epics**
- **E1.1** — Add EF Core persistence with write/read schemas and migrations

**Key outputs**
- EF Core DbContext + entity mapping for event store, idempotency, projections (DOC-05)
- Initial migration creates schemas/tables/indexes and `read.GlobalCounter` view (DOC-05)
- LocalDB connectivity and DB creation workflow (DOC-06)

---

### M2 — v3 Write Endpoints (Atomic + Idempotent)

**Goal:** Implement synchronous strong-consistency v3 write endpoints (Variant A).

**Epics**
- **E2.1** — Implement v3 counter write endpoints (atomic + idempotent)

**Key outputs**
- `POST /api/v3/counter` increments global counter (DOC-04)
- `POST /api/v3/counter/{userId}` increments global + user counters (DOC-04)
- One global increment event type with optional `UserId` (DOC-05)
- Operation-scoped idempotency prevents collisions (DOC-05)

---

### M3 — v3 Read Endpoints (Global + User)

**Goal:** Provide query endpoints backed by projection tables/views.

**Epics**
- **E3.1** — Implement v3 counter read endpoints

**Key outputs**
- `GET /api/v3/counter` returns global counter value (DOC-04)
- `GET /api/v3/counter/{userId}` returns global + user values (DOC-04)
- Reads use projections/views only (DOC-05)

---

### M4 — Integration Tests (LocalDB)

**Goal:** Validate correctness under real SQL behavior: atomicity, idempotency scoping, concurrency.

**Epics**
- **E4.1** — Integration tests with LocalDB (per-run DB + reset between tests)

**Key outputs**
- Per-run unique test DB provisioning and migrations (DOC-07)
- Reset strategy for deterministic tests (DOC-07)
- Core integration tests: global increment, user increment, idempotency scoping, concurrency (DOC-07)

---

### M5 — Observability, Health, Hardening

**Goal:** Add operational readiness and resilience features.

**Epics**
- **E5.1** — Add health checks and structured logging
- **E5.2** — Bounded retry for concurrency/transient SQL faults

**Key outputs**
- `/health/live` and `/health/ready` endpoints (DOC-08)
- Structured log enrichment and correlation fields (DOC-08)
- Bounded retry for transient/concurrency faults (DOC-08)

---

### M6 — Documentation & Polish

**Goal:** Ensure repo is self-serve for developers and agents; finalize runbook-quality docs.

**Epics**
- **E6.1** — Update README with local run, migrations, API usage

**Key outputs**
- README reflects LocalDB, migrations, scripts, v3 API usage patterns (DOC-04, DOC-06)
- Docs are consistent and complete (DOC-09)

## Cross-cutting conventions

- Architectural decisions are recorded as ADRs in `docs/decisions/`.
- If a decision changes an existing ADR, update that ADR and add a superseding ADR if needed.
- Agents must explicitly prompt for manual steps per [Manual Steps Checklist](MANUAL-STEPS.md).
