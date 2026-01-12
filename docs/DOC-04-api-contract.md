# DOC-04 — API Contract

## Base path

`/api/v3`

## Endpoints

### POST `/counter`

Increments the **global** counter.

**Headers**
- `Idempotency-Key: <string>` (required)

**Response (200)**
```json
{
  "globalValue": 1
}
```

**Semantics**
- `globalValue` is the monotonic global ordering number of the latest increment event (from `write.Events.Position`).

**Errors**
- `400` if `Idempotency-Key` is missing/blank
- `409` if a concurrency conflict cannot be resolved with bounded retries

---

### POST `/counter/{userId}`

Increments the **global** counter and the **per-user** counter.

**Route parameters**
- `userId` — GUID (required)

**Headers**
- `Idempotency-Key: <string>` (required)

**Response (200)**
```json
{
  "globalValue": 1,
  "userValue": 1
}
```

**Semantics**
- `globalValue` is the monotonic global ordering number of the latest increment event (from `write.Events.Position`).
- `userValue` is the per-user counter value from `read.UserCounters.Value` after the increment.

**Errors**
- `400` if `Idempotency-Key` is missing/blank
- `400` if `userId` is missing/invalid GUID
- `409` if a concurrency conflict cannot be resolved with bounded retries

---

### GET `/counter`

Returns the global counter value.

**Response (200)**
```json
{
  "globalValue": 123
}
```

---

### GET `/counter/{userId}`

Returns the global counter value and the per-user counter value.

**Route parameters**
- `userId` — GUID (required)

**Response (200)**
```json
{
  "globalValue": 123,
  "userId": "00000000-0000-0000-0000-000000000000",
  "userValue": 42
}
```

**Semantics**
- If the user has no recorded counter, return `userValue = 0`.

**Errors**
- `400` if `userId` is missing/invalid GUID

## Notes

- Authentication is out of scope. The client should provide a stable `userId` (persisted GUID).
- Both POST endpoints are synchronous and strongly consistent (Variant A).
