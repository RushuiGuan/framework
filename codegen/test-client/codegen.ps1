$location = get-location;
$project = $PSScriptRoot;

get-item $project\projects\test-webclient\src\lib\*.generated.ts | remove-item;

set-location $PSScriptRoot\..\Albatross.CodeGen.CommandLine;

dotnet run -- typescript-dto `
	-p $project\..\Test.Dto\Test.Dto.csproj `
	-s $project\..\Test.Proxy\codegen-settings.json `
	-o $project\projects\test-webclient\src\lib\ `
	-v information


dotnet run -- typescript-proxy `
	-p $project\..\Test.WebApi\Test.WebApi.csproj `
	-s $project\..\Test.Proxy\codegen-settings.json `
	-o $project\projects\test-webclient\src\lib\ `
	-v information

Set-Location $location;