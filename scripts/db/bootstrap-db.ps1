#!/usr/bin/env pwsh
# Apply EF Core migrations to LocalDB or a supplied connection string.

$ErrorActionPreference = "Stop"

param(
    [string]$ConnectionString
)

dotnet tool restore

$arguments = @(
    "ef", "database", "update",
    "--project", "src/TheButton.Infrastructure",
    "--startup-project", "src/TheButton.Api"
)

if (-not [string]::IsNullOrWhiteSpace($ConnectionString)) {
    $arguments += @("--connection", $ConnectionString)
} elseif (-not [string]::IsNullOrWhiteSpace($env:THEBUTTON_CONNECTIONSTRING)) {
    $arguments += @("--connection", $env:THEBUTTON_CONNECTIONSTRING)
}

& dotnet @arguments
