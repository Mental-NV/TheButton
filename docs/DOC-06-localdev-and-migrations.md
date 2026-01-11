# DOC-06 — Local Development & Migrations

## LocalDB

Use SQL Server LocalDB for local development:

- Server: `(localdb)\MSSQLLocalDB`
- Database: `TheButton`

Example connection string:

```text
Server=(localdb)\MSSQLLocalDB;Database=TheButton;Trusted_Connection=True;MultipleActiveResultSets=True
```

## Database creation

The database does not exist initially. It is created by applying EF Core migrations.

## Migrations policy

- Use EF Core migrations for schema management.
- Avoid `EnsureCreated()` for real environments.
- Recommended:
  - Development: optional auto-migrate on startup (guarded by environment)
  - Production: run migrations during deployment or via explicit operator command

## Local workflow (expected)

1. Restore/build
2. Apply migrations (or rely on dev auto-migrate if enabled)
3. Run the API and call endpoints

## Production notes (Azure SQL)

- Store the Azure SQL connection string in App Service configuration under `ConnectionStrings:Sql`.
- Apply migrations as part of deployment or via an operator-run step.
