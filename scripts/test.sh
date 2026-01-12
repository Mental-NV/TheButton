#!/bin/bash
# Test script for Linux/macOS (Bash)
# Runs build + unit + integration tests for both Backend and Mobile projects

set -e

run_tests() {
    local solution=$1
    echo "--- Processing $solution ---"
    
    echo "Running dotnet build for $solution..."
    dotnet build "$solution"

    echo "Running dotnet test for $solution..."
    dotnet test "$solution" --no-build
}

run_tests "TheButton.Backend.sln"
run_tests "TheButton.Mobile.sln"

echo "All tests passed for all solutions!"
exit 0
