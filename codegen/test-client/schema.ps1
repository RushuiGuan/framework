$location = get-location;
set-location $PSScriptRoot\..\Albatross.CodeGen.CommandLine;
dotnet run -- settings-schema > $location\codegen-settings.schema.json
Set-Location $location;