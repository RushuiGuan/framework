$location = "$PSScriptRoot\..\test-client\projects\test-webclient";

get-item $location\src\lib\*.generated.ts | remove-item;

codegen typescript-dto `
	-p $PSScriptRoot\..\Test.Dto\Test.Dto.csproj `
	-s $PSScriptRoot\codegen-settings.json `
	-o $location\src\lib\ `
	--log information


codegen typescript-proxy `
	-p $PSScriptRoot\..\Test.WebApi\Test.WebApi.csproj `
	-s $PSScriptRoot\codegen-settings.json `
	-o $location\src\lib\ `
	--log information

