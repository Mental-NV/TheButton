#!/bin/bash
set -e

# 1. Start Mock API in background (Bind to all interfaces)
dotnet run --project tools/TheButton.MockApi/TheButton.MockApi.csproj --urls "http://0.0.0.0:5001" > mock_api.log 2>&1 &
MOCK_API_PID=$!
echo "Mock API started with PID $MOCK_API_PID on http://0.0.0.0:5001"

# Function to cleanup background process
cleanup() {
  echo "--- Mock API Logs ---"
  cat mock_api.log
  echo "--- End Mock API Logs ---"

  echo "--- Logcat Crash Dump ---"
  adb logcat -d | grep -E "AndroidRuntime|FATAL|com.companyname.thebutton.mobile|dotnet" || true
  adb logcat -d | tail -n 100
  echo "--- End Logcat Crash Dump ---"

  echo "Saving full logcat to android-logcat.txt..."
  adb logcat -d > android-logcat.txt

  echo "Stopping Mock API (PID: $MOCK_API_PID)..."
  kill $MOCK_API_PID || true
}

# Trap exit signals to ensure cleanup
trap cleanup EXIT

# 2. Build & Sign APK (Debug) - E2E Config enabled via E2E_ANDROID_TEST constant
echo "Building APK..."
dotnet build src/TheButton.Mobile/TheButton.Mobile.csproj -f net10.0-android -r android-x64 -c Debug -p:AndroidPackageFormat=apk -p:EmbedAssembliesIntoApk=true -p:AndroidUseSharedRuntime=false -p:DefineConstants=E2E_ANDROID_TEST

# 4. Install APK
echo "Installing APK..."
adb install src/TheButton.Mobile/bin/Debug/net10.0-android/android-x64/com.companyname.thebutton.mobile-Signed.apk

# Wait for app readiness (simple sleep as app launch is handled by Maestro)
sleep 5

# 5. Run Maestro
echo "Running Maestro..."
$HOME/.maestro/bin/maestro test .maestro/android-flow.yaml
