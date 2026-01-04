# TheButton Project

## Production URLs
- Frontend: https://lively-water-053753610.2.azurestaticapps.net
- Backend API: https://clickthebutton.azurewebsites.net

## Prerequisites
- **.NET 10 SDK**
- **Node.js** (v18 or higher)

## Project Structure
- `src/TheButton.Api` - ASP.NET Core Backend API
- `src/TheButton.Web` - ReactJS Frontend (Vite)
- `src/TheButton.Mobile` - .NET MAUI Mobile App
- `src/TheButton.Core` - Shared Class Library

## Running the Application

### 1. Backend (API)
The backend runs on `http://localhost:5285`.

**Using CLI:**
```bash
# Navigate to the API project
cd src/TheButton.Api

# Run the project
dotnet run
```
You can access the API documentation (Scalar UI) at `http://localhost:5285/scalar/v1`.

### 2. Frontend (React)
The frontend runs on `http://localhost:5173` (by default).

**Using CLI:**
```bash
# Navigate to the Web project
cd src/TheButton.Web

# Install dependencies (first time only)
npm install

# Run the development server
npm run dev
```

## Running Tests
To run all unit and integration tests for the solution:
```bash
dotnet test
```

