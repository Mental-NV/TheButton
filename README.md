# TheButton.Mobile Runbook

## Prerequisites
- .NET SDK 10.0.100+
- MAUI Workload (`dotnet workload install maui`)
- Android Emulator (for Android) / Xcode (for iOS)

## Running Locally (Windows)
1. **Mock API**:
   ```powershell
   dotnet run --project tools/TheButton.MockApi/TheButton.MockApi.csproj
   ```
   (Runs on http://localhost:5001)

2. **Mobile App**:
   ```powershell
   dotnet build src/TheButton.Mobile/TheButton.Mobile.csproj -f net10.0-windows10.0.19041.0 -t:Run
   ```
   The app will use `appsettings.Development.json` pointing to `http://localhost:5285`.
   To use the Mock API, update `appsettings.Development.json` to `http://localhost:5001` or set environment variable `BaseApiUrl=http://localhost:5001`.

## Running Tests
### Unit Tests
```powershell
dotnet test tests/TheButton.Mobile.UnitTests/TheButton.Mobile.UnitTests.csproj
```

### Integration Tests
```powershell
dotnet test tests/TheButton.Mobile.IntegrationTests/TheButton.Mobile.IntegrationTests.csproj
```

## CI/CD
- **CI**: Runs on every push to main/PR. Validates build, unit tests, integration tests, and runs Maestro E2E on Android Emulator.
- **Release**: Runs on tags `v*`. Builds signed Android AAB and iOS IPA, and runs Maestro E2E on both platforms.

## Secrets Required (for Release)
- `ANDROID_KEYSTORE_BASE64`, `ANDROID_KEY_ALIAS`, `ANDROID_KEY_PASSWORD`, `ANDROID_KEYSTORE_PASSWORD`
- `IOS_P12_BASE64`, `IOS_P12_PASSWORD`, `IOS_PROVISIONING_PROFILE_BASE64`, `IOS_CODESIGN_KEY`, `IOS_PROVISIONING_PROFILE_NAME`
- `KEYCHAIN_PASSWORD` (Arbitrary password for temp keychain in CI)
