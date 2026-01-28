# 🕹️ TheButton.Web

A React + Vite front-end for **TheButton**. The app renders a single counter button, loads the current value from the API on start, and increments the counter on click while showing loading and error states.

## ✨ Features

- **Single-button counter UI** with loading and error states
- **Optimistic, idempotent increments** via API calls
- **Typed React hooks** for data access (`useButtonCounter`)

## 🔌 API Integration

The UI talks to the backend counter API:

- **GET** `${VITE_API_URL}/api/v3/counter` → load current count
- **POST** `${VITE_API_URL}/api/v3/counter` → increment count
  - Sends `Idempotency-Key` header for safe retries

For full backend details, see [`src/TheButton.Api/README.md`](../TheButton.Api/README.md).

## ⚙️ Environment Variables

Create a `.env` file (or export in your shell):

```bash
VITE_API_URL=http://localhost:5000
```

## 🛠️ Local Development

From `src/TheButton.Web/`:

```bash
npm install
npm run dev
```

Other useful commands:

- **Build**: `npm run build`
- **Lint**: `npm run lint`
- **Test**: `npm run test`
- **Watch tests**: `npm run test:watch`
- **Coverage**: `npm run test:coverage`

## 🧭 Key Files

- `src/App.tsx` – UI shell for the button
- `src/hooks/useButtonCounter.ts` – API integration and state management
- `src/App.test.tsx` – UI tests
