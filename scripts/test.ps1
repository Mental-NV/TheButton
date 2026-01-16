# Runs build + unit + integration tests for both Backend and Mobile projects
param(
    [switch]$Coverage
)

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
    $testArgs = @($solution, "--no-build")
    if ($Coverage) {
        $testArgs += "--collect:`"XPlat Code Coverage`""
        $testArgs += "--results-directory"
        $testArgs += "./TestResults"
    }

    dotnet test @testArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Tests failed for $solution!" -ForegroundColor Red
        exit $LASTEXITCODE
    }
}

Run-Tests "TheButton.Backend.sln"
Run-Tests "TheButton.Mobile.sln"

if ($Coverage) {
    Write-Host "Generating coverage report..." -ForegroundColor Cyan
    $reportGenerator = Get-Command reportgenerator -ErrorAction SilentlyContinue
    if ($reportGenerator) {
        reportgenerator "-reports:TestResults/**/coverage.cobertura.xml" "-targetdir:TestResults/CoverageReport" "-reporttypes:Html"
        Write-Host "Coverage report generated at: TestResults/CoverageReport/index.html" -ForegroundColor Green
    } else {
        Write-Host "reportgenerator not found. Skipping HTML report generation." -ForegroundColor Yellow
        Write-Host "You can install it using: dotnet tool install -g dotnet-reportgenerator-globaltool" -ForegroundColor Yellow
    }
}

Write-Host "All tests completed!" -ForegroundColor Green
exit 0
