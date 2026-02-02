#!/bin/bash
# Apply EF Core migrations to LocalDB or a supplied connection string.

set -e

dotnet tool restore

connection_string=""
if [ -n "$1" ]; then
  connection_string="$1"
elif [ -n "$THEBUTTON_CONNECTIONSTRING" ]; then
  connection_string="$THEBUTTON_CONNECTIONSTRING"
fi

args=(ef database update --project src/TheButton.Infrastructure --startup-project src/TheButton.Api)

if [ -n "$connection_string" ]; then
  args+=(--connection "$connection_string")
fi

dotnet "${args[@]}"
