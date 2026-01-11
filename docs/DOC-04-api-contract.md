# DOC-04 — API Contract

## Base path

`/api/v1`

## Endpoints

### POST `/counter/increment`

Increments the global counter and the per-user counter.

**Headers**
- `Idempotency-Key: <string>` (required)

**Body**
```json
{
  "userId": "00000000-0000-0000-0000-000000000000"
}
```

**Response (200)**
```json
{
  "globalValue": 1,
  "userValue": 1
}
```

**Semantics**
- `globalValue` is the monotonic global ordering number of the latest increment event (from `write.Events.Position`).
- `userValue` is the per-user counter value from `read.UserCounters.Value`.

**Errors**
- `400` if `Idempotency-Key` is missing/blank
- `400` if `userId` is missing/invalid GUID
- `409` if concurrency conflict cannot be resolved with bounded retries

---

### GET `/counter/global`

Returns global counter value.

**Response (200)**
```json
{
  "globalValue": 123
}
```

---

### GET `/counter/users/{userId}`

Returns per-user counter value.

**Response (200)**
```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "userValue": 42
}
```

**Semantics**
- If the user has no recorded counter, return `userValue = 0`.

## Notes

- Authentication is out of scope. The client should provide a stable `userId` (persisted GUID).
- Increment is synchronous and strongly consistent (Variant A).
