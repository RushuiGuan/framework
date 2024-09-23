$location = "$env:DevDirectory\test-client\projects\test-webclient";

get-item $location\*.generated.ts | remove-item;

codegen typescript-dto `
	-p $PSScriptRoot\..\Test.Dto\Test.Dto.csproj `
	-s $PSScriptRoot\codegen-settings.json `
	-o $location\src\lib\ `
	--log information