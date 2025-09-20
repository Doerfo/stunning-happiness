# .NET Weather API Application
This repository contains a .NET 9 sample application with a Weather API backend and Blazor Server-side frontend, designed for GitHub Codespaces development.

**ALWAYS follow these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

## Working Effectively

### Prerequisites and Setup
- Install .NET 9 SDK (required - .NET 8 is insufficient):
  - `curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 9.0`
  - `export PATH="$HOME/.dotnet:$PATH"`
  - Verify: `dotnet --version` should show 9.0.x

### Build and Run Commands
- **Restore dependencies**: `cd SampleApp && dotnet restore` -- takes 3 seconds
- **Build entire solution**: `cd SampleApp && dotnet build --configuration Release` -- takes 8 seconds, NEVER CANCEL, set timeout to 15+ seconds
- **Individual project builds**:
  - Backend: `dotnet build SampleApp/BackEnd/BackEnd.csproj --configuration Release` -- takes 1-2 seconds
  - Frontend: `dotnet build SampleApp/FrontEnd/FrontEnd.csproj --configuration Release` -- takes 1-2 seconds

### Run Applications
- **Backend API** (Weather API on port 8080):
  - `cd SampleApp/BackEnd && dotnet run --configuration Release --urls "http://localhost:8080"`
  - API endpoint: `http://localhost:8080/weatherforecast`
  - OpenAPI docs: `http://localhost:8080/scalar`
- **Frontend Blazor** (Weather UI on port 8081):
  - `cd SampleApp/FrontEnd && dotnet run --configuration Release --urls "http://localhost:8081"`
  - Web UI: `http://localhost:8081`
  - REQUIRES backend to be running for weather data

### Development with Hot Reload
- **Watch backend**: `dotnet watch run --project SampleApp/BackEnd/BackEnd.csproj --urls "http://localhost:8080"`
- **Watch frontend**: `dotnet watch run --project SampleApp/FrontEnd/FrontEnd.csproj --urls "http://localhost:8081"`

### VS Code Integration
- **Run both applications**: Use VS Code "Run All" compound configuration (F5 or Run menu)
- **Build tasks available**:
  - "build backend": `dotnet build SampleApp/BackEnd/BackEnd.csproj /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary`
  - "build frontend": `dotnet build SampleApp/FrontEnd/FrontEnd.csproj /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary`

## Validation and Testing

### Manual Validation Requirements
**ALWAYS run through complete end-to-end scenario after making changes:**
1. Start backend: `cd SampleApp/BackEnd && dotnet run --urls "http://localhost:8080"`
2. Verify API: `curl "http://localhost:8080/weatherforecast"` should return JSON weather data
3. Verify OpenAPI: Navigate to `http://localhost:8080/scalar` should show API documentation
4. Start frontend: `cd SampleApp/FrontEnd && dotnet run --urls "http://localhost:8081"`
5. Verify UI: Navigate to `http://localhost:8081` should show weather table with data from backend
6. Test data flow: Weather data in UI should match API responses

### CI/CD Validation
**Always run these commands to ensure CI compatibility:**
- `dotnet restore SampleApp/BackEnd/BackEnd.csproj` -- takes 1 second
- `dotnet build SampleApp/BackEnd/BackEnd.csproj --no-restore --configuration Release` -- takes 1 second
- `dotnet restore SampleApp/FrontEnd/FrontEnd.csproj` -- takes 1 second  
- `dotnet build SampleApp/FrontEnd/FrontEnd.csproj --no-restore --configuration Release` -- takes 1 second

### No Tests Available
- This repository contains **no test projects**
- `dotnet test` will complete successfully but run zero tests
- Focus validation on manual testing described above

## Project Structure and Navigation

### Key Directories
```
SampleApp/
├── BackEnd/              # Weather API (.NET 9 ASP.NET Core)
│   ├── Program.cs        # API endpoints and Scalar configuration  
│   └── BackEnd.csproj    # Dependencies: OpenAPI, Scalar.AspNetCore
├── FrontEnd/             # Blazor Server App (.NET 9)
│   ├── Program.cs        # Blazor configuration and HttpClient setup
│   ├── Data/             # WeatherForecastClient for API calls
│   ├── Pages/            # Razor pages and components
│   └── FrontEnd.csproj   # Blazor Server dependencies
└── SampleApp.sln         # Solution file for both projects
```

### Important Files
- **Backend API**: `SampleApp/BackEnd/Program.cs` - Weather API endpoints, OpenAPI/Scalar setup
- **Frontend Client**: `SampleApp/FrontEnd/Data/WeatherForecastClient.cs` - API client
- **Frontend Config**: `SampleApp/FrontEnd/appsettings.json` - Contains `WEATHER_URL: http://localhost:8080`
- **VS Code Config**: `.vscode/launch.json` - "Run All" compound configuration for both apps
- **CI Pipeline**: `.github/workflows/build.yml` - Builds both projects in matrix

### Port Configuration
- **Backend API**: Port 8080 (configurable via --urls)
- **Frontend UI**: Port 8081 (configurable via --urls)
- **Environment variable**: `WEATHER_URL=http://localhost:8080` in frontend appsettings.json

## Common Tasks

### After Code Changes
1. **Always test both applications** using the manual validation steps above
2. **Always run CI build commands** to ensure compatibility
3. **Check API responses** with curl before testing UI
4. **Verify Scalar documentation** still loads at /scalar endpoint

### Debugging Tips
- Both applications log startup info including listening URLs
- Backend will show "Now listening on: http://localhost:8080"
- Frontend will show "Now listening on: http://localhost:8081"
- Frontend requires backend to be running for weather data (will show errors if backend unavailable)

### GitHub Codespaces
- Repository is pre-configured for GitHub Codespaces
- Ports 8080 and 8081 are automatically forwarded
- .NET 9 SDK pre-installed in devcontainer
- Use "Run All" in VS Code to start both applications

## Timing and Performance

### Build Times (with 50% safety buffer)
- **Restore**: 3 seconds (timeout: 10+ seconds)
- **Full solution build**: 8 seconds (timeout: 15+ seconds)
- **Individual project build**: 1-2 seconds (timeout: 5+ seconds)
- **Application startup**: 1-2 seconds per application

### **CRITICAL**: NEVER CANCEL running builds or commands
- Builds may appear to hang but are processing
- Always wait for completion or explicit error messages
- Use appropriate timeouts but do not cancel prematurely