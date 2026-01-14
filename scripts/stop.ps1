Write-Host "Stopping TheButton processes..." -ForegroundColor Yellow

$titles = @("TheButton-API", "TheButton-Web", "TheButton-Mobile")

foreach ($title in $titles) {
    # Find processes where the window title contains our specific tag
    $procs = Get-Process | Where-Object { $_.MainWindowTitle -like "*$title*" }
    foreach ($proc in $procs) {
        Write-Host "Closing window: $($proc.MainWindowTitle)" -ForegroundColor Gray
        Stop-Process -Id $proc.Id -Force
    }
}

Write-Host "Done!" -ForegroundColor Green
