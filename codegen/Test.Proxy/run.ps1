# get-item $PSScriptRoot\*.generated.cs | remove-item;

codegen csharp-proxy `
	-p $PSScriptRoot\..\Test.WebApi\Test.WebApi.csproj `
	-s $PSScriptRoot\codegen-settings.json `
	-o $PSScriptRoot\