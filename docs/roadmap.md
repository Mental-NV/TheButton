# Roadmap

This roadmap serves as a high-level index for the project's milestones, epics, and backlog items.

## References

- [DOC-00 Index](DOC-00-index.md)
- [DOC-01 Agent Operating Instructions](DOC-01-agent-instructions.md)
- [DOC-02 Product Goals, Constraints, Non-Goals](DOC-02-goals-constraints-non-goals.md)
- [DOC-03 Architecture Overview](DOC-03-architecture-overview.md)
- [DOC-04 API Contract](DOC-04-api-contract.md)
- [DOC-05 Persistence Design](DOC-05-persistence-design.md)
- [DOC-06 Local Development & Migrations](DOC-06-localdev-and-migrations.md)
- [DOC-07 Integration Testing Strategy](DOC-07-integration-testing.md)
- [DOC-08 Observability & Reliability](DOC-08-observability-and-reliability.md)
- [DOC-09 Definition of Done](DOC-09-definition-of-done.md)

## Milestones

### M0 — Planning & Solution Restructure

- [EPIC: Introduce Clean Architecture solution structure](backlog/M0/E0-1/EPIC-M0-E0-1-introduce-clean-architecture-solution-structure.md)
  - [Add Asp.Versioning v3 route group conventions](backlog/M0/E0-1/TASK-M0-E0-1-add-asp-versioning-v3-route-group-conventions.md)
  - [Create new projects (Application/Domain/Infrastructure)](backlog/M0/E0-1/TASK-M0-E0-1-create-new-projects-application-domain-infrastructure.md)
  - [Minimal API mapping skeleton (feature-based)](backlog/M0/E0-1/TASK-M0-E0-1-minimal-api-mapping-skeleton-feature-based.md)
  - [Wire project references and dependency direction](backlog/M0/E0-1/TASK-M0-E0-1-wire-project-references-and-dependency-direction.md)
- [EPIC: Agent enablement (scripts, tools, explicit manual steps)](backlog/M0/E0-2/EPIC-M0-E0-2-agent-enablement-scripts-tools-explicit-manual-steps.md)
  - [Add docs/MANUAL-STEPS.md and enforce agent prompting](backlog/M0/E0-2/TASK-M0-E0-2-add-docs-manual-steps-md-and-enforce-agent-prompting.md)
  - [Add dotnet tool manifest with dotnet-ef](backlog/M0/E0-2/TASK-M0-E0-2-add-dotnet-tool-manifest-with-dotnet-ef.md)
  - [Add scripts/bootstrap-db.ps1 and scripts/bootstrap-db.sh](backlog/M0/E0-2/TASK-M0-E0-2-add-scripts-bootstrap-db-ps1-and-scripts-bootstrap-db-sh.md)
  - [Add scripts/test.ps1 and scripts/test.sh](backlog/M0/E0-2/TASK-M0-E0-2-add-scripts-test-ps1-and-scripts-test-sh.md)
- [EPIC: Restore V2 endpoints + tests (backward compatibility guardrail)](backlog/M0/E0-3/EPIC-M0-E0-3-restore-v2-endpoints-tests-backward-compatibility-guardrail.md)
  - [Restore V2 endpoints integration tests (keep V2 behavior stable)](backlog/M0/E0-3/TASK-M0-E0-3-restore-v2-endpoints-integration-tests-keep-v2-behavior-stable.md)

---

### M1 — Persistence Foundations (EF Core + Migrations)

- [EPIC: Add EF Core persistence with write/read schemas and migrations](backlog/M1/E1-1/EPIC-M1-E1-1-add-ef-core-persistence-with-write-read-schemas-and-migrations.md)
  - [Add ConnectionStrings:Sql for LocalDB and Azure SQL](backlog/M1/E1-1/TASK-M1-E1-1-add-connectionstrings-sql-for-localdb-and-azure-sql.md)
  - [Add EF Core SQL Server packages and DbContext skeleton](backlog/M1/E1-1/TASK-M1-E1-1-add-ef-core-sql-server-packages-and-dbcontext-skeleton.md)
  - [Add migration SQL for read.GlobalCounter view](backlog/M1/E1-1/TASK-M1-E1-1-add-migration-sql-for-read-globalcounter-view.md)
  - [Create initial migration for schemas/tables/indexes](backlog/M1/E1-1/TASK-M1-E1-1-create-initial-migration-for-schemas-tables-indexes.md)
  - [Define event schema: CounterIncremented with optional UserId](backlog/M1/E1-1/TASK-M1-E1-1-define-event-schema-counterincremented-with-optional-userid.md)
  - [Define idempotency schema scoped by operation and user](backlog/M1/E1-1/TASK-M1-E1-1-define-idempotency-schema-scoped-by-operation-and-user.md)
  - [Dev-only auto-migrate on startup (optional)](backlog/M1/E1-1/TASK-M1-E1-1-dev-only-auto-migrate-on-startup-optional.md)
  - [Map entities for write.Events/write.Commands/read.UserCounters](backlog/M1/E1-1/TASK-M1-E1-1-map-entities-for-write-events-write-commands-read-usercounters.md)
  - [Update DOC-05 to reflect optional userId + idempotency scoping](backlog/M1/E1-1/TASK-M1-E1-1-update-doc-05-to-reflect-optional-userid-idempotency-scoping.md)
- [EPIC: CI split (backend/web vs mobile) to avoid MAUI workloads on backend changes](backlog/M1/E1-2/EPIC-M1-E1-2-ci-split-backend-web-vs-mobile-to-avoid-maui-workloads-on-backend-changes.md)
  - [Implement CI workflow split (path filtering) backend/web vs mobile](backlog/M1/E1-2/TASK-M1-E1-2-implement-ci-workflow-split-path-filtering-backend-web-vs-mobile.md)

---

### M2 — Increment Write Path (Atomic + Idempotent)

- [EPIC: Implement v3 counter write endpoints (atomic + idempotent)](backlog/M2/E2-1/EPIC-M2-E2-1-implement-v3-counter-write-endpoints-atomic-idempotent.md)
  - [Add GlobalIncrement command/handler/result](backlog/M2/E2-1/TASK-M2-E2-1-add-globalincrement-command-handler-result.md)
  - [Add Minimal API endpoint POST /api/v3/counter/{userId}](backlog/M2/E2-1/TASK-M2-E2-1-add-minimal-api-endpoint-post-api-v3-counter-userid.md)
  - [Add Minimal API endpoint POST /api/v3/counter](backlog/M2/E2-1/TASK-M2-E2-1-add-minimal-api-endpoint-post-api-v3-counter.md)
  - [Add UserIncrement command/handler/result](backlog/M2/E2-1/TASK-M2-E2-1-add-userincrement-command-handler-result.md)
  - [Define Application abstractions for counter writer and reads](backlog/M2/E2-1/TASK-M2-E2-1-define-application-abstractions-for-counter-writer-and-reads.md)
  - [Implement SQL counter writer (single-transaction, two modes)](backlog/M2/E2-1/TASK-M2-E2-1-implement-sql-counter-writer-single-transaction-two-modes.md)
  - [Remove in-memory counter service and legacy controller routes](backlog/M2/E2-1/TASK-M2-E2-1-remove-in-memory-counter-service-and-legacy-controller-routes.md)

---

### M3 — Read Endpoints (Global + Per-User)

- [EPIC: Implement v3 counter read endpoints](backlog/M3/E3-1/EPIC-M3-E3-1-implement-v3-counter-read-endpoints.md)
  - [Add Minimal API endpoint GET /api/v3/counter/{userId}](backlog/M3/E3-1/TASK-M3-E3-1-add-minimal-api-endpoint-get-api-v3-counter-userid.md)
  - [Add Minimal API endpoint GET /api/v3/counter](backlog/M3/E3-1/TASK-M3-E3-1-add-minimal-api-endpoint-get-api-v3-counter.md)
  - [Implement SQL read repository (global and per-user)](backlog/M3/E3-1/TASK-M3-E3-1-implement-sql-read-repository-global-and-per-user.md)

---

### M4 — Integration Tests (LocalDB)

- [EPIC: Integration tests with LocalDB (per-run DB + reset)](backlog/M4/E4-1/EPIC-M4-E4-1-integration-tests-with-localdb-per-run-db-reset.md)
  - [CI: ensure integration tests run on Windows with LocalDB](backlog/M4/E4-1/TASK-M4-E4-1-ci-ensure-integration-tests-run-on-windows-with-localdb.md)
  - [Create integration test project and test server factory](backlog/M4/E4-1/TASK-M4-E4-1-create-integration-test-project-and-test-server-factory.md)
  - [Implement DB reset utility (truncate tables)](backlog/M4/E4-1/TASK-M4-E4-1-implement-db-reset-utility-truncate-tables.md)
  - [Integration test: concurrency (N parallel global increments)](backlog/M4/E4-1/TASK-M4-E4-1-integration-test-concurrency-n-parallel-global-increments.md)
  - [Integration test: concurrency (N parallel user increments)](backlog/M4/E4-1/TASK-M4-E4-1-integration-test-concurrency-n-parallel-user-increments.md)
  - [Integration test: idempotency scoped by operation and user](backlog/M4/E4-1/TASK-M4-E4-1-integration-test-idempotency-scoped-by-operation-and-user.md)
  - [Integration test: POST /counter (global) increments and persists event](backlog/M4/E4-1/TASK-M4-E4-1-integration-test-post-counter-global-increments-and-persists-event.md)
  - [Integration test: POST /counter/{userId} increments user and persists event](backlog/M4/E4-1/TASK-M4-E4-1-integration-test-post-counter-userid-increments-user-and-persists-event.md)
  - [Provision per-run unique LocalDB database and apply migrations](backlog/M4/E4-1/TASK-M4-E4-1-provision-per-run-unique-localdb-database-and-apply-migrations.md)

---

### M5 — Observability, Health, Hardening

- [EPIC: Observability and health](backlog/M5/E5-1/EPIC-M5-E5-1-observability-and-health.md)
  - [Add health check endpoints (live/ready)](backlog/M5/E5-1/TASK-M5-E5-1-add-health-check-endpoints-live-ready.md)
  - [Add structured logging enrichment for requests](backlog/M5/E5-1/TASK-M5-E5-1-add-structured-logging-enrichment-for-requests.md)
- [EPIC: Bounded retry for concurrency/transient SQL faults](backlog/M5/E5-2/EPIC-M5-E5-2-bounded-retry-for-concurrency-transient-sql-faults.md)
  - [Add bounded retry in SQL counter writer](backlog/M5/E5-2/TASK-M5-E5-2-add-bounded-retry-in-sql-counter-writer.md)
  - [Optional: stress integration test validating retry behavior](backlog/M5/E5-2/TASK-M5-E5-2-optional-stress-integration-test-validating-retry-behavior.md)

---

### M6 — Documentation & Polish

- [EPIC: Documentation updates (README + examples)](backlog/M6/E6-1/EPIC-M6-E6-1-documentation-updates-readme-examples.md)
  - [README: API examples (curl) for v3 endpoints](backlog/M6/E6-1/TASK-M6-E6-1-readme-api-examples-curl-for-v3-endpoints.md)
  - [README: LocalDB + migrations + scripts section](backlog/M6/E6-1/TASK-M6-E6-1-readme-localdb-migrations-scripts-section.md)
  - [Update roadmap and ADR references after v3 contract change](backlog/M6/E6-1/TASK-M6-E6-1-update-roadmap-and-adr-references-after-v3-contract-change.md)
