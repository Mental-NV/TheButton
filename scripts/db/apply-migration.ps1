dotnet ef database update `
    --project src/TheButton.Infrastructure `
    --startup-project src/TheButton.Api `
    --connection $env:THEBUTTON_AZURESQL_CONNECTIONSTRING
