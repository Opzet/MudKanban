$version="0.1.0"
cd src/MudKanban
dotnet pack -p:PackageVersion=$version
nuget push bin/Release/MudKanban.${version}.nupkg -Source https://api.nuget.org/v3/index.json
cd ../..
