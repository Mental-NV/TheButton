# Manual Steps Checklist

This repository is designed to maximize implementation by coding AI agents, but some steps require explicit human action (or at least explicit confirmation).

## Rule for agents

When any step below is required, the agent must:

1. **Stop** the implementation sequence at an appropriate boundary.
2. **Explicitly tell the user** what command/action to run.
3. **Ask the user to confirm the result** (paste output or confirm success) before continuing.

## EF Core migrations (generation and application)

### When required
- Any time the EF Core model changes in a way that requires a new migration:
  - adding/removing tables/columns/indexes/views
  - changing schema names
  - changing constraints (including filtered indexes)

### Expected human actions
1. Restore tools (once per environment):
   - `dotnet tool restore`
2. Create a migration (example):
   - `dotnet ef migrations add <MigrationName> -p src/TheButton.Infrastructure -s src/TheButton.Api`
3. Apply to a database (example LocalDB):
   - `dotnet ef database update -p src/TheButton.Infrastructure -s src/TheButton.Api`

### Notes
- Agents can author SQL and migration code, but **tool-generated migrations** (including snapshot files) are best produced by running `dotnet ef migrations add`.

## Azure provisioning and secrets

### When required
- First-time deployment
- Rotating credentials
- Changing connection string names/keys
- Configuring app settings for Azure SQL

### Expected human actions
- Create/verify Azure resources (Resource Group, App Service, Azure SQL, firewall rules)
- Set secrets and app settings in Azure:
  - `ConnectionStrings:Sql` (Azure SQL connection string)
  - any other required config values

### Notes
- Agents can provide CLI commands and scripts, but the user must run them and confirm success.

## GitHub repository settings

### When required
- When adding required checks / branch protections
- When configuring environments and secrets for CI/CD

### Expected human actions
- Configure repository secrets (if needed)
- Confirm CI runners and permissions behave as expected
