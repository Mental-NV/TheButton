#!/usr/bin/env pwsh
# Test script for Windows (PowerShell)
# Runs build + unit + integration tests for both Backend and Mobile projects

$ErrorActionPreference = "Stop"

function Run-Tests($solution) {
    Write-Host "--- Processing $solution ---" -ForegroundColor Cyan
    
    Write-Host "Running dotnet build for $solution..." -ForegroundColor Cyan
    dotnet build $solution
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Build failed for $solution!" -ForegroundColor Red
        exit $LASTEXITCODE
    }

    Write-Host "Running dotnet test for $solution..." -ForegroundColor Cyan
    dotnet test $solution --no-build
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Tests failed for $solution!" -ForegroundColor Red
        exit $LASTEXITCODE
    }
}

Run-Tests "TheButton.Backend.sln"
Run-Tests "TheButton.Mobile.sln"

Write-Host "Running npm tests for TheButton.Web..." -ForegroundColor Cyan
npm --prefix src\TheButton.Web test
if ($LASTEXITCODE -ne 0) {
    Write-Host "npm tests failed for TheButton.Web!" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "All tests passed for all solutions!" -ForegroundColor Green
exit 0
