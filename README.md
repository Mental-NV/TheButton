# TheButton

TheButton is a demonstration project showcasing modern, industrial-grade software development practices for building, testing, and deploying cross-platform applications (API, Web UI, iOS, and Android) using a unified architectural approach.

## Deployments

[![Backend Coverage](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/Mental-NV/TheButton/main/.github/badges/coverage.json)](https://github.com/Mental-NV/TheButton/actions/workflows/ci-backend.yml)
[![CI Backend](https://github.com/Mental-NV/TheButton/actions/workflows/ci-backend.yml/badge.svg)](https://github.com/Mental-NV/TheButton/actions/workflows/ci-backend.yml)
[![CI Mobile](https://github.com/Mental-NV/TheButton/actions/workflows/ci-mobile.yml/badge.svg)](https://github.com/Mental-NV/TheButton/actions/workflows/ci-mobile.yml)
[![Mobile E2E](https://github.com/Mental-NV/TheButton/actions/workflows/e2e-mobile.yml/badge.svg)](https://github.com/Mental-NV/TheButton/actions/workflows/e2e-mobile.yml)
[![Azure Deploy](https://github.com/Mental-NV/TheButton/actions/workflows/deploy.yml/badge.svg)](https://github.com/Mental-NV/TheButton/actions/workflows/deploy.yml)

-   **Web (Frontend)**: https://lively-water-053753610.2.azurestaticapps.net
-   **API (Backend)**: https://clickthebutton.azurewebsites.net

## Project Overview

-   `src/TheButton.Api`: ASP.NET Core Web API (Backend)
-   `src/TheButton.Web`: React application built with Vite (Frontend)
-   `src/mobile/TheButton.Mobile`: .NET MAUI application (Mobile)
-   `tests/*`: Comprehensive testing suits for all major components


## Prerequisites

-   **.NET SDK**: 10.0.101+
-   **Node.js**: LTS version (for Web)
-   **MAUI Workload**: `dotnet workload install maui`
-   **Mobile Emulators**:
    -   Android Emulator (via Android Studio)
    -   Xcode (for iOS, macOS only)

---

## 🔌 API (Backend)

The backend is an ASP.NET Core Web API located in `src/TheButton.Api`.

### Run Locally

```bash
dotnet run --project src/TheButton.Api/TheButton.Api.csproj
```

-   **API URL**: `http://localhost:5285` (Configured in `launchSettings.json`)
-   **Scalar/OpenAPI UI**: `http://localhost:5285/scalar/v1`

### LocalDB, Migrations, and Scripts

The API uses SQL Server LocalDB by default.

```text
Server=(localdb)\MSSQLLocalDB;Database=TheButton;Trusted_Connection=True;MultipleActiveResultSets=True
```

Restore tools and apply migrations:

```bash
dotnet tool restore
dotnet ef database update -p src/TheButton.Infrastructure -s src/TheButton.Api
```

Or use the bootstrap scripts:

```powershell
.\scripts\db\bootstrap-db.ps1
```

```bash
./scripts/db/bootstrap-db.sh
```

To target a custom connection string, set `THEBUTTON_CONNECTIONSTRING` before running the script.

### API Examples (v3)

Global increment:

```bash
curl -X POST "http://localhost:5285/api/v3/counter" \
  -H "Idempotency-Key: 11111111-1111-1111-1111-111111111111"
```

Response:

```json
{
  "value": 1,
  "userValue": null
}
```

User increment:

```bash
curl -X POST "http://localhost:5285/api/v3/counter/aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" \
  -H "Idempotency-Key: 22222222-2222-2222-2222-222222222222"
```

Response:

```json
{
  "value": 2,
  "userValue": 1
}
```

Global read:

```bash
curl "http://localhost:5285/api/v3/counter"
```

Response:

```json
{
  "value": 2,
  "userValue": null
}
```

User read:

```bash
curl "http://localhost:5285/api/v3/counter/aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"
```

Response:

```json
{
  "value": 2,
  "userValue": 1
}
```

### Test

Run all unit and integration tests:

```bash
dotnet test
```

---

## 🚀 Web (Frontend)

The frontend is a React application built with Vite, located in `src/TheButton.Web`.

### Setup

```bash
cd src/TheButton.Web
npm install
```

### Run Locally

```bash
npm run dev --host
```

Access the app at `http://localhost:5173`.

### Test

```bash
npm test            # Run all tests once
npm run test:watch  # Run in watch mode
npm run test:coverage # Run with coverage report
```

---

## 📱 Mobile

The mobile app is built with .NET MAUI, located in `src/mobile/TheButton.Mobile`.

### Running Locally (Windows)

The mobile app is configured to connect to the local API by default.

1.  **Start the Backend**:
    Run `TheButton.Api` (it runs on `http://localhost:5285` which the mobile app expects).

2.  **Run the Mobile App**:
    ```powershell
    dotnet build src/mobile/TheButton.Mobile/TheButton.Mobile.csproj -f net10.0-windows10.0.19041.0 -t:Run
    ```
    *Note: The app uses `appsettings.Development.json` which is configured to point to `http://localhost:5285`.*

### Tests

**Unit Tests**:

```powershell
dotnet test tests/mobile/TheButton.Mobile.UnitTests/TheButton.Mobile.UnitTests.csproj
```

**Integration Tests**:

```powershell
dotnet test tests/mobile/TheButton.Mobile.IntegrationTests/TheButton.Mobile.IntegrationTests.csproj
```

---

## ⚙️ CI/CD

Hosted on GitHub Actions.

-   **CI Backend**: Triggers on push to `main` and PRs (excluding mobile paths). Validates .NET Backend and Web.
-   **CI Mobile**: Triggers on push to `main` and PRs (mobile paths only). Validates .NET MAUI Mobile app.
-   **Mobile E2E**: Triggers on mobile-related changes. Runs Maestro E2E tests on Android and iOS.
-   **Deploy**: Triggers after successful `CI` run on `main`. Deploys API and Web components to Azure.
