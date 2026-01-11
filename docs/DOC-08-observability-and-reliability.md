# DOC-08 — Observability & Reliability

## Logging

- Use structured logging.
- Include correlation fields where possible:
  - request trace id
  - `userId`
  - `Idempotency-Key`
  - latency/elapsed time
- Log retries for increment transaction with attempt count.

## Health checks

Expose two endpoints:

- `/health/live`
  - Indicates process is running

- `/health/ready`
  - Indicates application is ready to serve requests
  - Must include DB connectivity check

## Resilience

### Transient faults

- Enable SQL retry strategy where appropriate.
- Handle transient SQL errors with bounded retry.

### Concurrency

- If optimistic concurrency conflicts occur during increment, implement bounded retry:
  - 2–3 attempts
  - log attempts
  - fail with `409` if unrecoverable

## Performance considerations (baseline)

- Event store table is append-only and clustered by identity position.
- Read endpoints hit projections/views only.
- Keep increment transaction minimal and avoid unnecessary reads.
