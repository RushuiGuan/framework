$location = get-location

get-item $PSScriptRoot\*.Generated.cs | remove-item;
set-location $PSScriptRoot\..\Sample.CodeGen
dotnet run create-api-csharp-proxy --out $PSScriptRoot --namespace Sample.Proxy
set-location $location;

