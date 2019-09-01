import-module app-dev;
Get-Childitem -Path $PSScriptRoot -File -Filter *.csproj | ForEach-Object {
	Publish-LocalNuget $_ debug;
}
