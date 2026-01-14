# DOC-04 — API Contract

## Base path

`/api/v3`

## Endpoints

### POST `/counter`

Increments the **global** counter and optionally the **per-user** counter.

**Query Parameters**
- `userId` — GUID (optional). If provided, increments the user-specific counter.

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
- `userValue` is the per-user counter value (calculated as `MAX(UserVersion)` for the provided `userId`) after the increment. It is `null` if no `userId` was provided.

**Errors**
- `400` if `Idempotency-Key` is missing/blank
- `400` if `userId` is provided but is an invalid GUID
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
