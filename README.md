# TheButton

TheButton is a demonstration project showcasing modern, industrial-grade software development practices for building, testing, and deploying cross-platform applications (Web, iOS, and Android) using a unified architectural approach.

## Deployments

[![CI](https://github.com/Mental-NV/TheButton/actions/workflows/ci.yml/badge.svg)](https://github.com/Mental-NV/TheButton/actions/workflows/ci.yml)
[![Mobile E2E](https://github.com/Mental-NV/TheButton/actions/workflows/e2e-mobile.yml/badge.svg)](https://github.com/Mental-NV/TheButton/actions/workflows/e2e-mobile.yml)
[![Azure Deploy](https://github.com/Mental-NV/TheButton/actions/workflows/deploy.yml/badge.svg)](https://github.com/Mental-NV/TheButton/actions/workflows/deploy.yml)

-   **Web (Frontend)**: https://lively-water-053753610.2.azurestaticapps.net
-   **API (Backend)**: https://clickthebutton.azurewebsites.net

## Project Overview

-   `src/TheButton.Api`: ASP.NET Core Web API (Backend)
-   `src/TheButton.Web`: React application built with Vite (Frontend)
-   `src/TheButton.Mobile`: .NET MAUI application (Mobile)
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

The mobile app is built with .NET MAUI, located in `src/TheButton.Mobile`.

### Running Locally (Windows)

The mobile app is configured to connect to the local API by default.

1.  **Start the Backend**:
    Run `TheButton.Api` (it runs on `http://localhost:5285` which the mobile app expects).

2.  **Run the Mobile App**:
    ```powershell
    dotnet build src/TheButton.Mobile/TheButton.Mobile.csproj -f net10.0-windows10.0.19041.0 -t:Run
    ```
    *Note: The app uses `appsettings.Development.json` which is configured to point to `http://localhost:5285`.*

### Tests

**Unit Tests**:

```powershell
dotnet test tests/TheButton.Mobile.UnitTests/TheButton.Mobile.UnitTests.csproj
```

**Integration Tests**:

```powershell
dotnet test tests/TheButton.Mobile.IntegrationTests/TheButton.Mobile.IntegrationTests.csproj
```

---

## ⚙️ CI/CD

Hosted on GitHub Actions.

-   **CI**: Triggers on push to `main` and PRs. Validates .NET (API/Mobile) and runs tests.
-   **Mobile E2E**: Triggers on mobile-related changes. Runs Maestro E2E tests on Android and iOS.
-   **Deploy**: Triggers after successful `CI` run on `main`. Deploys API and Web components to Azure.
