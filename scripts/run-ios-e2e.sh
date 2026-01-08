#!/bin/bash
set -euo pipefail

# ============================================================================
# iOS E2E Test Runner for GitHub Actions
# Runs Maestro tests against iOS Simulator with Mock API
# ============================================================================

echo "=== Environment Info ==="
sw_vers
xcodebuild -version
xcrun simctl --version
dotnet --version
echo "========================"

# ============================================================================
# 1. Start Mock API in background
# ============================================================================
echo "Starting Mock API..."
dotnet run --project tools/TheButton.MockApi/TheButton.MockApi.csproj \
  --urls "http://localhost:5001" > mock_api.log 2>&1 &
MOCK_API_PID=$!
echo "Mock API started (PID: $MOCK_API_PID) on http://localhost:5001"

# Wait for Mock API to be ready
sleep 5

# ============================================================================
# Cleanup handler
# ============================================================================
cleanup() {
  local exit_code=$?
  echo ""
  echo "=== Cleanup Starting ==="
  
  echo "--- Mock API Logs ---"
  cat mock_api.log 2>/dev/null || echo "(no logs)"
  echo "--- End Mock API Logs ---"
  
  echo "Stopping Mock API (PID: $MOCK_API_PID)..."
  kill $MOCK_API_PID 2>/dev/null || true
  
  # Shutdown and delete simulator if it exists
  if [ -n "${SIM_UDID:-}" ]; then
    echo "Cleaning up simulator $SIM_UDID..."
    xcrun simctl shutdown "$SIM_UDID" 2>/dev/null || true
    xcrun simctl delete "$SIM_UDID" 2>/dev/null || true
  fi
  
  echo "=== Cleanup Complete ==="
  exit $exit_code
}
trap cleanup EXIT

# ============================================================================
# 2. Create and boot iOS Simulator
# ============================================================================
DEVICE_TYPE="iPhone 16"
RUNTIME="iOS18.6"
SIM_NAME="E2E-iPhone-CI"

echo "Setting up iOS Simulator..."
echo "  Device Type: $DEVICE_TYPE"
echo "  Runtime: $RUNTIME"

# List available runtimes for debugging
echo "Available runtimes:"
xcrun simctl list runtimes

# Clean existing E2E simulator if present
xcrun simctl delete "$SIM_NAME" 2>/dev/null || true

# Create fresh simulator
echo "Creating simulator '$SIM_NAME'..."
SIM_UDID=$(xcrun simctl create "$SIM_NAME" "$DEVICE_TYPE" "$RUNTIME")
echo "Created simulator with UDID: $SIM_UDID"

# Boot simulator and wait for ready
echo "Booting simulator..."
xcrun simctl boot "$SIM_UDID"
xcrun simctl bootstatus "$SIM_UDID" -b
echo "Simulator booted and ready"

# ============================================================================
# 3. Build iOS Simulator App
# ============================================================================
echo ""
echo "=== Building iOS Simulator App ==="
dotnet build src/TheButton.Mobile/TheButton.Mobile.csproj \
  -f net10.0-ios \
  -c Debug \
  -r iossimulator-arm64 \
  -p:DefineConstants=E2E_IOS_TEST \
  -v minimal

# Find the .app bundle
APP_PATH=$(find src/TheButton.Mobile/bin/Debug/net10.0-ios/iossimulator-arm64 -name "*.app" -type d | head -1)
if [ -z "$APP_PATH" ]; then
  echo "ERROR: Could not find .app bundle"
  find src/TheButton.Mobile/bin -name "*.app" -type d || true
  exit 1
fi
echo "Built app: $APP_PATH"

# ============================================================================
# 4. Install App on Simulator
# ============================================================================
echo ""
echo "=== Installing App ==="
xcrun simctl install "$SIM_UDID" "$APP_PATH"
echo "App installed successfully"

# ============================================================================
# 5. Run Maestro Tests
# ============================================================================
echo ""
echo "=== Running Maestro Tests ==="
echo "MAESTRO_DRIVER_STARTUP_TIMEOUT=${MAESTRO_DRIVER_STARTUP_TIMEOUT:-not set}"

# Ensure Maestro output directory exists
mkdir -p ~/.maestro/tests

# Run Maestro with explicit UDID and output
~/.maestro/bin/maestro --device "$SIM_UDID" test .maestro/ios-flow.yaml \
  --test-output-dir ~/.maestro/tests \
  --format JUNIT \
  --output ~/.maestro/tests/junit.xml

echo ""
echo "=== iOS E2E Tests Complete ==="