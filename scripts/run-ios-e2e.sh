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
# You can override these from the workflow/job/step env.
# - IOS_RUNTIME_MAJOR: preferred iOS major version (e.g., "18", "26")
# - IOS_DEVICE_TYPE_NAME: preferred device name (e.g., "iPhone 16")
# - IOS_SIM_NAME: simulator name (default: E2E-iPhone-CI)
SIM_NAME="${IOS_SIM_NAME:-E2E-iPhone-CI}"
IOS_RUNTIME_MAJOR="${IOS_RUNTIME_MAJOR:-18}"
IOS_DEVICE_TYPE_NAME="${IOS_DEVICE_TYPE_NAME:-iPhone 16}"

echo "Setting up iOS Simulator..."
echo "  Preferred device name: $IOS_DEVICE_TYPE_NAME"
echo "  Preferred iOS major:   $IOS_RUNTIME_MAJOR"

# List available runtimes/devicetypes for debugging (human-readable)
echo "Available runtimes:"
xcrun simctl list runtimes || true
echo "Available device types:"
xcrun simctl list devicetypes || true

resolve_ios_runtime_id() {
  python3 - "$IOS_RUNTIME_MAJOR" <<'PY'
import json, subprocess, sys, re

major = int(sys.argv[1])

def parse_ver(v: str):
    parts = []
    for p in (v or "0").split("."):
        try:
            parts.append(int(p))
        except ValueError:
            parts.append(0)
    while len(parts) < 3:
        parts.append(0)
    return tuple(parts[:3])

def load_simctl_json(args):
    out = subprocess.check_output(args, text=True, stderr=subprocess.STDOUT)
    # Some toolchains occasionally emit warnings before JSON. Keep JSON from first '{'.
    i = out.find("{")
    if i > 0:
        out = out[i:]
    try:
        return json.loads(out)
    except json.JSONDecodeError:
        snippet = out[:400].replace("\n", "\\n")
        print(f"SIMCTL_JSON_PARSE_ERROR: {snippet}", file=sys.stderr)
        raise

data = load_simctl_json(["xcrun","simctl","list","runtimes","-j"])
rts = [
    r for r in data.get("runtimes", [])
    if r.get("isAvailable")
    and r.get("identifier","").startswith("com.apple.CoreSimulator.SimRuntime.iOS-")
]
if not rts:
    sys.exit(2)

preferred = [r for r in rts if parse_ver(r.get("version","0"))[0] == major]
pool = preferred if preferred else rts
pool.sort(key=lambda r: parse_ver(r.get("version","0")))
print(pool[-1]["identifier"])
PY
}

resolve_iphone_devicetype_id() {
  python3 - "$IOS_DEVICE_TYPE_NAME" <<'PY'
import json, subprocess, sys, re

preferred_name = sys.argv[1]

out = subprocess.check_output(["xcrun","simctl","list","devicetypes","-j"], text=True, stderr=subprocess.STDOUT)
i = out.find("{")
if i > 0:
    out = out[i:]
data = json.loads(out)

dts = data.get("devicetypes", [])

# Exact match first
for d in dts:
    if d.get("name") == preferred_name:
        print(d.get("identifier",""))
        sys.exit(0)

# Fallback: newest iPhone by number (best-effort)
iphones = [d for d in dts if (d.get("name") or "").startswith("iPhone")]
def rank(name: str):
    m = re.search(r"iPhone\s+(\d+)", name or "")
    return int(m.group(1)) if m else -1

iphones.sort(key=lambda d: rank(d.get("name","")))
if iphones:
    print(iphones[-1].get("identifier",""))
    sys.exit(0)

sys.exit(3)
PY
}

RUNTIME_ID="$(resolve_ios_runtime_id)" || {
  echo "ERROR: Failed to resolve iOS runtime (major=$IOS_RUNTIME_MAJOR)."
  echo "Diagnostics:"
  xcrun simctl list runtimes || true
  exit 1
}

DEVICE_TYPE_ID="$(resolve_iphone_devicetype_id)" || {
  echo "ERROR: Failed to resolve device type ($IOS_DEVICE_TYPE_NAME)."
  echo "Diagnostics:"
  xcrun simctl list devicetypes || true
  exit 1
}

echo "Resolved Runtime ID:     $RUNTIME_ID"
echo "Resolved Device Type ID: $DEVICE_TYPE_ID"

# Clean existing E2E simulator if present
xcrun simctl delete "$SIM_NAME" 2>/dev/null || true

# Create fresh simulator using identifiers (most reliable)
echo "Creating simulator '$SIM_NAME'..."
SIM_UDID="$(xcrun simctl create "$SIM_NAME" "$DEVICE_TYPE_ID" "$RUNTIME_ID")"
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
~/.maestro/bin/maestro test .maestro/ios-flow.yaml \
  --udid "$SIM_UDID" \
  --output ~/.maestro/tests

echo ""
echo "=== iOS E2E Tests Complete ==="
