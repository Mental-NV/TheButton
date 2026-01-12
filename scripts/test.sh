#!/bin/bash
# Test script for Linux/macOS (Bash)
# Runs build + unit + integration tests

set -e

echo "Running dotnet build..."
dotnet build

echo "Running dotnet test..."
dotnet test --no-build

echo "All tests passed!"
exit 0
