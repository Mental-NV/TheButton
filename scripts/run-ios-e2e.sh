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
# 2. Create and boot iOS Simulator (robust runtime/device selection)
# ============================================================================
SIM_NAME="${IOS_SIM_NAME:-E2E-iPhone-CI}"
IOS_DEVICE_TYPE_NAME="${IOS_DEVICE_TYPE_NAME:-iPhone 16}"
IOS_RUNTIME_MAJOR="${IOS_RUNTIME_MAJOR:-18}"

echo "Setting up iOS Simulator..."
echo "  Preferred device name: $IOS_DEVICE_TYPE_NAME"
echo "  Preferred iOS major:   $IOS_RUNTIME_MAJOR"

# List available runtimes/devicetypes for debugging
echo "Available runtimes:"
xcrun simctl list runtimes
echo "Available device types:"
xcrun simctl list devicetypes

# Resolve runtime identifier (prefer latest available in the desired major; else latest overall)
RUNTIME_LINE="$(xcrun simctl list runtimes -j | python3 - "$IOS_RUNTIME_MAJOR" <<'PY'
import json, sys

desired_major = int(sys.argv[1])
data = json.load(sys.stdin)

def parse_ver(v: str):
    parts = []
    for p in (v or "").split("."):
        try:
            parts.append(int(p))
        except ValueError:
            parts.append(0)
    while len(parts) < 3:
        parts.append(0)
    return tuple(parts[:3])

rts = [
    r for r in data.get("runtimes", [])
    if r.get("isAvailable")
    and (r.get("identifier","").startswith("com.apple.CoreSimulator.SimRuntime.iOS-"))
]

if not rts:
    print("")
    sys.exit(0)

# Prefer desired major if present (e.g., 18.x)
preferred = [r for r in rts if parse_ver(r.get("version","0"))[0] == desired_major]
pool = preferred if preferred else rts

pool.sort(key=lambda r: parse_ver(r.get("version","0")))
best = pool[-1]
print(f'{best["identifier"]}|{best.get("name","")}|{best.get("version","")}')
PY
)"

if [[ -z "$RUNTIME_LINE" || "$RUNTIME_LINE" != *"|"* ]]; then
  echo "ERROR: Could not resolve an iOS runtime."
  exit 1
fi

RUNTIME_ID="${RUNTIME_LINE%%|*}"
REST="${RUNTIME_LINE#*|}"
RUNTIME_NAME="${REST%%|*}"
RUNTIME_VERSION="${REST#*|}"

# Resolve device type identifier (prefer requested name; else newest iPhone)
DEVICE_TYPE_ID="$(xcrun simctl list devicetypes -j | python3 - "$IOS_DEVICE_TYPE_NAME" <<'PY'
import json, sys, re

preferred_name = sys.argv[1]
data = json.load(sys.stdin)
dts = data.get("devicetypes", [])

# Exact match first
for d in dts:
    if d.get("name") == preferred_name:
        print(d.get("identifier",""))
        sys.exit(0)

# Fallback: newest iPhone by number
iphones = [d for d in dts if (d.get("name") or "").startswith("iPhone")]
def iphone_rank(name: str):
    m = re.search(r"iPhone\s+(\d+)", name or "")
    return int(m.group(1)) if m else -1

iphones.sort(key=lambda d: iphone_rank(d.get("name","")))
print((iphones[-1].get("identifier","")) if iphones else (dts[0].get("identifier","") if dts else ""))
PY
)"

if [ -z "$DEVICE_TYPE_ID" ]; then
  echo "ERROR: Could not resolve an iPhone device type."
  exit 1
fi

echo "Resolved runtime: $RUNTIME_NAME ($RUNTIME_VERSION)"
echo "  Runtime ID:     $RUNTIME_ID"
echo "Resolved device type ID: $DEVICE_TYPE_ID"

# Clean existing simulator if present
xcrun simctl delete "$SIM_NAME" 2>/dev/null || true

# Create fresh simulator using identifiers (most reliable)
echo "Creating simulator '$SIM_NAME'..."
SIM_UDID="$(xcrun simctl create "$SIM_NAME" "$DEVICE_TYPE_ID" "$RUNTIME_ID")"
echo "Created simulator with UDID: $SIM_UDID"

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
~/.maestro/bin/maestro test .maestro/ios-flow.yaml \
  --udid "$SIM_UDID" \
  --output ~/.maestro/tests

echo ""
echo "=== iOS E2E Tests Complete ==="
