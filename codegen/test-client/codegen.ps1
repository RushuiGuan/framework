$location = $PSScriptRoot;

get-item $location\projects\test-webclient\src\lib\*.generated.ts | remove-item;

codegen typescript-dto `
	-p $location\..\Test.Dto\Test.Dto.csproj `
	-s $location\..\Test.Proxy\codegen-settings.json `
	-o $location\projects\test-webclient\src\lib\ `
	--log information


codegen typescript-proxy `
	-p $location\..\Test.WebApi\Test.WebApi.csproj `
	-s $location\..\Test.Proxy\codegen-settings.json `
	-o $location\projects\test-webclient\src\lib\ `
	--log information

