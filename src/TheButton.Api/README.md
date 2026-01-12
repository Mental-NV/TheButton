# 🔌 TheButton.Api

The backend for **TheButton** is an ASP.NET Core Web API built with .NET 10. It provides high-performance counter management with built-in versioning and documentation.

## 🏗️ Project Structure

The project follows a **Vertical Slices** (Feature-Folders) architecture to ensure high cohesion and low coupling:

```text
src/TheButton.Api/
├── Abstractions/           # Infrastructure abstractions (e.g., IEndpoint)
├── Extensions/             # Dependency injection and mapping extensions
├── Features/               # Vertical Slices (Core Business Logic)
│   ├── Health/             # Self-contained health check feature
│   └── V2/
│       └── Counter/        # Counter management (Endpoints, Models)
├── Properties/             # Environmental settings (launchSettings.json)
├── Program.cs              # Entry point and automated endpoint registration
└── TheButton.Api.csproj    # Project manifest and NuGet packages
```

---

## 🚀 API Reference

The API uses **versioning** and provides interactive documentation via **Scalar**.

### Standard Endpoints
- **Health Check**: `/health` - Returns the status of the API.

### Featured Endpoints
- **Counter (V2)**:
    - **Base Route**: `/api/v2/counter`
    - **POST /**: Increments the counter and returns the new value.

### Documentation
- **Scalar UI**: `/scalar/v1` (Available in Development mode)
- **OpenAPI Spec**: `/openapi/v1.json`

---

## ⚙️ CI/CD Pipelines

The project uses GitHub Actions for automated validation and deployment.

### 🧪 Continuous Integration (`ci.yml`)
The CI pipeline triggers on every push and pull request to the `main` branch. It ensures that the code compiles and tests pass.

```yaml
# .github/workflows/ci.yml (API Snippet)
jobs:
  dotnet:
    name: .NET build & test
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          global-json-file: global.json
      - name: Restore
        run: dotnet restore TheButton.sln
      - name: Build
        run: dotnet build TheButton.sln -c Release --no-restore
      - name: Test
        run: dotnet test TheButton.sln -c Release --no-build
```

### 🚢 Continuous Deployment (`deploy.yml`)
The deployment pipeline triggers after a successful CI run on `main`. It publishes the API and deploys it to Azure App Service.

```yaml
# .github/workflows/deploy.yml (API Snippet)
jobs:
  deploy_backend:
    name: Deploy backend (Azure Web App)
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
      - name: Publish
        run: dotnet publish src/TheButton.Api/TheButton.Api.csproj -c Release -o ./publish
      - name: Deploy
        uses: azure/webapps-deploy@v2
        with:
          app-name: clickthebutton
          publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
          package: ./publish
```

---

## 🛠️ Local Development

### Prerequisites
- **.NET SDK**: 10.0.101 (specified in `global.json`)

### Commands
- **Run**: `dotnet run --project src/TheButton.Api/TheButton.Api.csproj`
- **Test**: `dotnet test`
