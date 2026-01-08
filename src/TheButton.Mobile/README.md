# TheButton.Mobile

## Project Overview

**TheButton.Mobile** is a multi-platform .NET MAUI application that targets Android, iOS, and Windows. It allows users to interact with "The Button" - a simple feature that increments a global counter.

### Architecture & Design Patterns

The project follows a clean architecture approach with the **MVVM (Model-View-ViewModel)** pattern:

- **TheButton.Mobile**: The UI layer (Views).
  - Contains the MAUI app bootstrapping (`MauiProgram.cs`).
  - Defines the Views (e.g., `MainPage.xaml`).
  - Configuration (appsettings.json) and Resource management.
- **TheButton.Mobile.Core**: The Application layer.
  - Contains **ViewModels** (e.g., `MainViewModel.cs`) which handle the presentation logic and state.
  - Defines **Core Interfaces** (e.g., `IButtonApiClient`).
  - Not dependent on any UI-specific implementation details.
- **TheButton.Mobile.Infrastructure**: The Infrastructure layer.
  - Implements external concerns such as API Clients (`ButtonApiClient`).
  - Registered via Dependency Injection in `MauiProgram.cs`.

This structure promotes testability and separation of concerns (`View` -> `ViewModel` -> `Model/Service`).

## Build & Run

### Prerequisites

- .NET 8.0 SDK (or compatible version as per global.json)
- MAUI Workload (`dotnet workload install maui`)
- **Android**: Android SDK & Emulator (or device)
- **iOS**: Xcode (Mac only)
- **Windows**: Visual Studio 2022 / VS Code with Windows App SDK

### Local Development

To run the application locally on different platforms:

**Windows:**
```bash
dotnet build src/TheButton.Mobile/TheButton.Mobile.csproj -f net10.0-windows10.0.19041.0
dotnet run --project src/TheButton.Mobile/TheButton.Mobile.csproj -f net10.0-windows10.0.19041.0
```

**Android:** (Requires Emulator or Device)
```bash
dotnet build src/TheButton.Mobile/TheButton.Mobile.csproj -f net10.0-android
dotnet build src/TheButton.Mobile/TheButton.Mobile.csproj -t:Run -f net10.0-android
```

**iOS:** (Mac Only)
```bash
dotnet build src/TheButton.Mobile/TheButton.Mobile.csproj -f net10.0-ios
dotnet build src/TheButton.Mobile/TheButton.Mobile.csproj -t:Run -f net10.0-ios
```

### Conditional Building

The project project file (`TheButton.Mobile.csproj`) is configured with **Conditional TargetFrameworks** to optimize the build process and support specific CI environments:

- **Windows**: Builds `net10.0-windows...` (Android is excluded to prevent build errors if SDKs are missing)
- **macOS**: Builds `net10.0-ios` and `net10.0-android`
- **Linux** (CI): Builds `net10.0-android` (Specifically for Android E2E tests in CI)

## Testing & CI

### Automated Testing

The project uses **Maestro** for End-to-End (E2E) UI testing.

- **Mock API**: E2E tests often run against a mock API (`TheButton.MockApi`) to ensure deterministic behavior.
- **Test Flows**: Defined in `.maestro/` (e.g., `android-flow.yaml`, `ios-flow.yaml`).

### CI Workflow (`ci.yml`)

The GitHub Actions CI pipeline ensures code quality and functional integrity:

1.  **Build & Test (Windows)**:
    - Builds the entire solution.
    - Runs unit tests.
2.  **Android E2E (Ubuntu)**:
    - Sets up an Android Emulator (API 33).
    - Installs **Maestro**.
    - Runs the project on the emulator.
    - Executes Maestro flows to verify UI interactions.
    - Uploads test reports and Logcat logs on failure.

## Release Workflow (`release.yml`)

Triggered on tagging a release (e.g., `v1.0.0`).

1.  **Android Release**:
    - Builds and Signs an Android App Bundle (`.aab`) using a Keystore stored in GitHub Secrets.
    - Artifact: `android-release-aab`.
2.  **iOS Release** (Mac):
    - Decodes Apple Certificates (.p12) and Provisioning Profiles.
    - Builds and Signs an iOS Application Archive (`.ipa`).
    - Artifact: `ios-release-ipa`.
3.  **E2E Verification**:
    - Runs a final E2E verification on **iOS Simulator** using a Mock API to ensure the release build (in simulator config) functions correctly before distribution.
