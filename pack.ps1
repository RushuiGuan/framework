get-childitem $PSScriptRoot\src\*.csproj -r | foreach-object { 
	dotnet pack $_.FullName --output c:\temp --configuration release 
}