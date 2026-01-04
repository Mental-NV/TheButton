# TheButton Project

This repository contains a small sample system with three primary apps:

- `src/TheButton.Api` — ASP.NET Core Web API (POST `/api/button/click`).
- `src/TheButton.Web` — React + Vite frontend (reads `VITE_API_URL`).
- `src/TheButton.Mobile` — .NET MAUI single-project mobile app (Android, iOS, MacCatalyst, Windows).

## Production URLs
- Frontend: https://lively-water-053753610.2.azurestaticapps.net
- Backend API: https://clickthebutton.azurewebsites.net

## Prerequisites

- .NET 10 SDK
- MAUI workload (for mobile): `dotnet workload install maui`
- Node.js 18+ and npm (for the frontend)
- Platform SDKs as needed (Xcode on macOS for iOS, Android SDK/emulator for Android, Visual Studio on Windows for MAUI/Windows builds)

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

### Mobile (.NET MAUI)

The project targets multiple TFMs: `net10.0-android`, `net10.0-ios`, `net10.0-maccatalyst`, and (on Windows) `net10.0-windows10.0.19041.0`. Use the appropriate TFM below.

Install MAUI workload once if needed:

```bash
dotnet workload install maui
```

Android (emulator or device)

```bash
# Ensure an emulator is running or a device is connected
dotnet build src/TheButton.Mobile -f net10.0-android
dotnet run --project src/TheButton.Mobile -f net10.0-android
```

iOS (macOS only — simulator or device)

```bash
# On macOS with Xcode installed
dotnet build src/TheButton.Mobile -f net10.0-ios
dotnet run --project src/TheButton.Mobile -f net10.0-ios
```

MacCatalyst (macOS desktop)

```bash
dotnet run --project src/TheButton.Mobile -f net10.0-maccatalyst
```

Windows (when on Windows)

Prerequisites:

- Visual Studio 2022/2023 with the MAUI workload (recommended) or Windows SDK + developer mode enabled.

CLI (from repo root):

```bash
dotnet build src/TheButton.Mobile -f net10.0-windows10.0.19041.0
dotnet run --project src\TheButton.Mobile -f net10.0-windows10.0.19041.0
```

For debugging and packaging on Windows, open the solution in Visual Studio and run (F5).

## Tests

Run unit and integration tests for the solution:

```bash
dotnet test TheButton.sln
```

## CI / CD

See `.github/workflows/deploy.yml` — the workflow uses .NET 10, installs the MAUI workload, builds and tests the solution, publishes the backend artifact and deploys the frontend to Azure Static Web Apps.




