# DOC-09 — Definition of Done

A milestone is complete when:

## Functional correctness

- Increment endpoint matches DOC-04 and is strongly consistent (Variant A).
- Read endpoints match DOC-04 and read projections/views.

## Architecture constraints

- Minimal APIs are used for new endpoints (no controllers).
- CQRS is implemented without MediatR.
- Clean Architecture layering is respected:
  - Application does not depend on Infrastructure
  - Api is composition root

## Persistence

- EF Core migrations define and create schemas/tables/views per DOC-05.
- LocalDB works from a clean state (DB created by migrations).

## Testing

- Integration tests run successfully and cover:
  - atomic increment behavior
  - idempotency
  - concurrency

## Operational readiness

- Health endpoints exist (`/health/live`, `/health/ready`).
- Logs include correlation fields for increment requests.

## Documentation

- Docs updated if contract or architecture changes.
- README includes local run and migration workflow (as planned in roadmap).
