# DOC-07 — Integration Testing Strategy (LocalDB)

## Purpose

Validate correctness of:

- transactional atomicity (event + projection + command result)
- idempotency behavior under retries
- concurrency behavior under parallel increments

All against a real SQL engine (LocalDB in CI on Windows).

## Recommended database strategy

- Create **one unique database per test run** (e.g., `TheButton_Tests_{GUID}`)
- Apply EF Core migrations once for that database
- Between tests, reset data by truncating/deleting rows while preserving schema

### Rationale

- Faster than drop/create schema per test
- More reliable than transaction rollback across HTTP boundaries (API uses its own connections)

## Test harness

- Use `WebApplicationFactory<Program>` to host the API for integration tests
- Override configuration to inject the test DB connection string
- Provide a database reset utility used per test

## Tables to reset between tests

- `write.Commands`
- `write.Events`
- `read.UserCounters`

## Minimum integration test suite

1. **Increment persists event and projection**
   - Call `POST /counter/increment`
   - Assert:
     - exactly one event exists
     - user counter value is incremented to 1
     - response `globalValue` equals inserted `Position`

2. **Idempotency prevents double increment**
   - Call increment twice with same `Idempotency-Key`
   - Assert:
     - only one event exists
     - response is identical

3. **Concurrency**
   - Fire N parallel increment requests for same `UserId`
   - Assert:
     - `userValue == N`
     - event count increased by N
     - uniqueness of `(StreamId, StreamVersion)` is preserved

## CI compatibility

The current CI runs on Windows runners, which supports LocalDB.
