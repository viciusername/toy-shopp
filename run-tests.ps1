# run-tests.ps1
# Usage: Open PowerShell in repository root and run: .\run-tests.ps1

$root = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location $root

# Create solution if missing
if (-not (Get-ChildItem -Filter *.sln -Recurse -ErrorAction SilentlyContinue)) {
    dotnet new sln -n ToyPlanet
}

# Create classlib project if missing
if (-not (Test-Path "src/ToyPlanet.Core/ToyPlanet.Core.csproj")) {
    dotnet new classlib -o src/ToyPlanet.Core -n ToyPlanet.Core
    # Move existing source files into project folder if necessary
}

# Create xunit test project if missing
if (-not (Test-Path "tests/ToyPlanet.Tests/ToyPlanet.Tests.csproj")) {
    dotnet new xunit -o tests/ToyPlanet.Tests -n ToyPlanet.Tests
    # Move existing test files into tests project folder if necessary
}

# Add projects to solution (ignore errors if already added)
dotnet sln add src/ToyPlanet.Core/ToyPlanet.Core.csproj 2>$null
dotnet sln add tests/ToyPlanet.Tests/ToyPlanet.Tests.csproj 2>$null

# Add project reference from tests to src (ignore errors if already set)
dotnet add tests/ToyPlanet.Tests/ToyPlanet.Tests.csproj reference src/ToyPlanet.Core/ToyPlanet.Core.csproj 2>$null

# Run tests and save console output and TRX results
$logFile = Join-Path $root "test_output.txt"
Write-Host "Running tests..."
dotnet test --logger "trx;LogFileName=TestResults.trx" | Tee-Object -FilePath $logFile
Write-Host "`nTest output saved to $logFile and TestResults.trx"
