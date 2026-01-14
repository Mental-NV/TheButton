# Start API
Write-Host "Starting API..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "`$Host.UI.RawUI.WindowTitle = 'TheButton-API'; dotnet run --project '$PSScriptRoot/../src/TheButton.Api/TheButton.Api.csproj'"

# Start Web
Write-Host "Starting Web..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "`$Host.UI.RawUI.WindowTitle = 'TheButton-Web'; Set-Location '$PSScriptRoot/../src/TheButton.Web'; npm run dev --host"

# Start Mobile
Write-Host "Starting Mobile..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "`$Host.UI.RawUI.WindowTitle = 'TheButton-Mobile'; dotnet build '$PSScriptRoot/../src/mobile/TheButton.Mobile/TheButton.Mobile.csproj' -f net10.0-windows10.0.19041.0 -t:Run"
