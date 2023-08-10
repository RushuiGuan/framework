$location = get-location

set-location $PSScriptRoot\..\SampleProject.CodeGen
dotnet run create-api-csharp-proxy --out $PSScriptRoot --namespace SampleProject.Proxy
set-location $location;

