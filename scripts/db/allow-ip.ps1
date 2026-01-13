$RG_NAME = "BookGenerator"
$SQL_SERVER_NAME = "bookgenerator"

$MY_IP = (Invoke-RestMethod -Uri "https://api.ipify.org")
az sql server firewall-rule create `
    --resource-group $RG_NAME `
    --server $SQL_SERVER_NAME `
    --name "AllowLocalIP" `
    --start-ip-address $MY_IP `
    --end-ip-address $MY_IP
