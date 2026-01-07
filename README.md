# TheButton Project

This repository contains a small sample system with three primary apps:

- `src/TheButton.Api` — ASP.NET Core Web API (POST `/api/button/click`).
- `src/TheButton.Web` — React + Vite frontend (reads `VITE_API_URL`).

## Production URLs
- Frontend: https://lively-water-053753610.2.azurestaticapps.net
- Backend API: https://clickthebutton.azurewebsites.net

## Prerequisites

- .NET 10 SDK
- Node.js 18+ and npm (for the frontend)

## Build

Build the full solution from the repository root:

```bash
dotnet build TheButton.sln --configuration Debug
```

## Run

### API (backend)

Runs on `http://localhost:5285` by default (see `src/TheButton.Api/Properties/launchSettings.json`).

From repo root:

```bash
dotnet run --project src/TheButton.Api
```

Open the Scalar/OpenAPI UI in development at `http://localhost:5285/scalar/v1`.


### Web (frontend)

Dev server:

```bash
cd src/TheButton.Web
npm install   # first time only
npm run dev
```

Default dev URL: `http://localhost:5173`

## Tests

Run unit and integration tests for the solution:

```bash
dotnet test TheButton.sln
```

## CI / CD

See `.github/workflows/deploy.yml` — the workflow uses .NET 10, installs the MAUI workload, builds and tests the solution, publishes the backend artifact and deploys the frontend to Azure Static Web Apps.




