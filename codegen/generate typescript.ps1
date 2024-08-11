$location = get-location

set-location $psscriptroot\Albatross.CodeGen.Utility


$output = "C:\app\my-client-api\my-client-api\src";
get-childitem $output\lib -filter *.ts | remove-item;

dotnet run typescript-proxy `
	--project-file ..\Albatross.CodeGen.Tests.WebApi\Albatross.CodeGen.Tests.WebApi.csproj --output-directory $output\lib `
	--settings "C:\app\my-client-api\code-gen-settings.json"
set-location $location;