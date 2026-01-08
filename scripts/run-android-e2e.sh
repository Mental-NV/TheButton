#!/bin/bash
set -e

# 1. Start Mock API in background
dotnet run --project tools/TheButton.MockApi/TheButton.MockApi.csproj --urls "http://localhost:5001" > mock_api.log 2>&1 &
MOCK_API_PID=$!
echo "Mock API started with PID $MOCK_API_PID"

# Function to cleanup background process
cleanup() {
  echo "Stopping Mock API (PID: $MOCK_API_PID)..."
  kill $MOCK_API_PID || true
}

# Trap exit signals to ensure cleanup
trap cleanup EXIT

# 2. Configure App to use 10.0.2.2:5001 (Emulator loopback to host)
echo '{"BaseApiUrl": "http://10.0.2.2:5001/"}' > src/TheButton.Mobile/appsettings.Development.json

# 3. Build & Sign APK (Debug)
echo "Building APK..."
dotnet build src/TheButton.Mobile/TheButton.Mobile.csproj -f net10.0-android -c Debug -p:AndroidPackageFormat=apk

# 4. Install APK
echo "Installing APK..."
adb install src/TheButton.Mobile/bin/Debug/net10.0-android/com.companyname.thebutton.mobile-Signed.apk

# 5. Run Maestro
echo "Running Maestro..."
$HOME/.maestro/bin/maestro test .maestro/android-flow.yaml
