# DOC-04 — API Contract

## Base path

`/api/v3`

## Endpoints

### POST `/counter`

Increments the **global** counter only.

**Headers**
- `Idempotency-Key: <string>` (optional, auto-generated if not provided)

**Response (200)**
```json
{
  "value": 1,
  "userValue": null
}
```

**Semantics**
- `value` is the monotonic global ordering number of the latest increment event (from `write.Events.Position`).
- `userValue` is always `null` for this endpoint.

**Errors**
- `409` if a concurrency conflict cannot be resolved with bounded retries

---

### POST `/counter/{userId}`

Increments the **global** counter and the **per-user** counter.

**Route parameters**
- `userId` — GUID (required)

**Headers**
- `Idempotency-Key: <string>` (optional, auto-generated if not provided)

**Response (200)**
```json
{
  "value": 1,
  "userValue": 1
}
```

**Semantics**
- `value` is the monotonic global ordering number of the latest increment event (from `write.Events.Position`).
- `userValue` is the per-user counter value (calculated as `MAX(UserVersion)` for the provided `userId`) after the increment.

**Errors**
- `400` if `userId` is an invalid GUID
- `409` if a concurrency conflict cannot be resolved with bounded retries

---

### GET `/counter`

Returns the global counter value.

**Response (200)**
```json
{
  "value": 123,
  "userValue": null
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
  "value": 123,
  "userValue": 42
}
```

**Semantics**
- If the user has no recorded counter, return `userValue = 0`.
- `userValue` is calculated by querying the `write.Events` table for the target `userId`.

**Errors**
- `400` if `userId` is missing/invalid GUID

## Notes

- Authentication is out of scope. The client should provide a stable `userId` (persisted GUID).
- POST `/counter` is synchronous and strongly consistent (Variant A).

---

## Legacy Version (v2)

### Base path

`/api/v2`

### POST `/counter`

Increments the **global** counter using the legacy in-memory implementation. (For backward compatibility only).

**Response (200)**
```json
{
  "value": 1
}
```
