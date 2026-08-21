$version="0.1.0"
cd src/MudKanban
dotnet pack -p:PackageVersion=$version
nuget push bin/Release/MudKanban.${version}.nupkg -Source https://api.nuget.org/v3/index.json
cd ../..
#!/usr/bin/env pwsh
# Builds and packs the MudKanban NuGet package.
param(
    [string]$Configuration = "Release",
    [string]$OutputDir = "./artifacts"
)

$ErrorActionPreference = "Stop"

Write-Host "Building MudKanban in $Configuration mode..." -ForegroundColor Cyan
dotnet build src/MudKanban/MudKanban.csproj -c $Configuration

Write-Host "Packing NuGet package..." -ForegroundColor Cyan
dotnet pack src/MudKanban/MudKanban.csproj -c $Configuration -o $OutputDir --no-build

Write-Host "Package written to $OutputDir" -ForegroundColor Green
Get-ChildItem $OutputDir -Filter "*.nupkg"
