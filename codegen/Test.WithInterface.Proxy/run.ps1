get-item $PSScriptRoot\*.generated.cs | remove-item;

$location = Get-Location

try {
	set-Location $PSScriptRoot\..\Albatross.CodeGen.CommandLine

	dotnet run -- csharp-proxy `
		-p $PSScriptRoot\..\Test.WebApi\Test.WebApi.csproj `
		-s $PSScriptRoot\codegen-settings.json `
		-o $PSScriptRoot\
} finally {
	set-Location $location
}