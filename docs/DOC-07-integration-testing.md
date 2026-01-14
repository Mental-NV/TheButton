# DOC-07 — Integration Testing Strategy (LocalDB)

## Purpose

Validate correctness of:

- transactional atomicity (event + command result)
- idempotency behavior (including user scoping)
- concurrency behavior under parallel increments (global and per-user)

All against a real SQL engine (LocalDB in CI on Windows).

## Recommended database strategy

- Create **one unique database per test run** (e.g., `TheButton_Tests_{GUID}`).
- Apply EF Core migrations once for that database.
- Between tests, reset data by truncating/deleting rows while preserving schema.

### Rationale

- Faster than drop/create schema per test.
- More reliable than transaction rollback across HTTP boundaries (API uses its own connections).

## Test harness

- Use `WebApplicationFactory<Program>` to host the API for integration tests.
- Override configuration to inject the test DB connection string.
- Provide a database reset utility used per test.

## Tables to reset between tests

- `write.Commands`
- `write.Events`

## Minimum integration test suite (v3 contract)

1. **POST /api/v3/counter increments global and persists event**
   - Call `POST /api/v3/counter` with `Idempotency-Key`.
   - Assert:
     - exactly one `write.Events` row with `UserId IS NULL`
     - response `globalValue == write.Events.Position`
     - one `write.Commands` row for `(Operation='Increment', UserId=NULL, IdempotencyKey=...)`

2. **POST /api/v3/counter?userId={guid} increments user and persists event**
   - Call `POST /api/v3/counter?userId={userId}` with `Idempotency-Key`.
   - Assert:
     - `MAX(UserVersion) == 1` for `{userId}` in `write.Events`
     - one `write.Events` row with `UserId=<userId>` and `UserVersion==1`
     - response contains `{globalValue, userValue}`

3. **Idempotency prevents double increment**
   - Repeat the same request twice (with or without `userId`) with the same `Idempotency-Key`:
     - only one event is persisted
     - `globalValue` and `userValue` (if applicable) are the same in both responses
   - Assert response payloads from `write.Commands.ResultJson` are identical.

4. **Idempotency is scoped by user**
   - Use the same `Idempotency-Key` for:
     - `POST /api/v3/counter` (global) and
     - `POST /api/v3/counter?userId={userId}` (user)
   - Assert no collisions:
     - both operations persist distinct command rows (due to different `UserId` in `write.Commands`)
     - two distinct events are persisted

5. **Concurrency (N parallel user increments)**
   - Fire N parallel `POST /api/v3/counter?userId={userId}` calls with distinct idempotency keys.
   - Assert:
     - `MAX(UserVersion) == N` for `{userId}` in `write.Events`
     - N events with `UserId=<userId>`
     - uniqueness holds for `(UserId, UserVersion)`

6. **Concurrency (N parallel global increments)**
   - Fire N parallel `POST /api/v3/counter` calls with distinct idempotency keys.
   - Assert:
     - N events with `UserId IS NULL`
     - `MAX(Position)` advances by N (relative to starting point)

## CI compatibility

- CI must run on Windows runners to use LocalDB.
- Prefer `scripts/test.*` as the single entry point for CI test execution.

## Manual steps note

If migrations need to be **generated** (not just applied), follow [Manual Steps Checklist](MANUAL-STEPS.md) and have the agent explicitly prompt the user to run the required `dotnet ef migrations add` command.
