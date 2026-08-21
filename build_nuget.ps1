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
