#!/usr/bin/env pwsh
# Test script for Windows (PowerShell)
# Runs build + unit + integration tests

$ErrorActionPreference = "Stop"

Write-Host "Running dotnet build..." -ForegroundColor Cyan
dotnet build
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "Running dotnet test..." -ForegroundColor Cyan
dotnet test --no-build
if ($LASTEXITCODE -ne 0) {
    Write-Host "Tests failed!" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "All tests passed!" -ForegroundColor Green
exit 0
