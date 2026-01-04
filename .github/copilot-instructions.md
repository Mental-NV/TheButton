# Copilot Instructions for TheButton
## Prerequisites
- .NET 10 SDK
- Node.js 18+ (for the React app)

## Big picture
- Solution contains four projects:
  - src/TheButton.Api (ASP.NET Core Web API) exposes POST api/button/click that returns { value }.
  - src/TheButton.Web (React + Vite) calls the API via VITE_API_URL.
  - src/TheButton.Mobile (.NET MAUI) currently increments a local counter only.
  - src/TheButton.Core (Class Library) reserved for shared code (currently minimal).
- API uses dependency injection and a singleton `ICounterService` implemented by `CounterService` (thread-safe in-memory counter). No persistence.
- CORS policy `AllowFrontend` allows origin http://localhost:5173 (Vite dev server).
- OpenAPI/Scalar UI is enabled in Development at /scalar/v1.

## Local workflows
- Backend (port http://localhost:5285):
  - Run: dotnet run --project src/TheButton.Api
  - Endpoints: POST /api/button/click returns `{ value: number }`.
- Frontend (Vite):
  - In src/TheButton.Web, create .env.local with VITE_API_URL=http://localhost:5285
  - Install and run: npm install; npm run dev (default http://localhost:5173)
- Mobile (MAUI):
  - Builds with standard MAUI template; not wired to API yet.

## Tests
- Run all: dotnet test TheButton.sln
- Unit tests cover controller and service behavior; integration tests use `WebApplicationFactory<Program>`.
- Note: `CounterService` is a singleton; integration tests assert increasing values, not exact counts.

## Conventions and patterns
- Controllers live under src/TheButton.Api/Controllers; routes start with api/* and use `[ApiController]`.
- Services live under src/TheButton.Api/Services; register in Program.cs via `builder.Services.*`.
- DTOs/records under src/TheButton.Api/Models (e.g., `CounterResponse`).
- Thread-safety: mutable state uses locks as in `CounterService`.
- Frontend expects API JSON shape `{ value }` and invokes `${VITE_API_URL}/api/button/click` via fetch.
- CORS: update `AllowFrontend` origins if frontend dev URL changes.

## CI/CD
- GitHub Actions (.github/workflows/deploy.yml):
  - Windows build: sets up .NET 10 SDK, installs MAUI workload, builds and tests solution.
  - Publishes backend and deploys to Azure Web App using `AZURE_WEBAPP_PUBLISH_PROFILE`.
  - Builds frontend and deploys to Azure Static Web Apps using `AZURE_STATIC_WEB_APPS_API_TOKEN`.

## Examples
- Add a new API endpoint: create a controller in src/TheButton.Api/Controllers, inject services via constructor, return DTOs/records from src/TheButton.Api/Models, and register needed services in Program.cs.
- Frontend API call pattern: read `import.meta.env.VITE_API_URL` and expect `{ value }` in response.
