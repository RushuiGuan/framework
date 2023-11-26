param(
	[switch]
	[bool]$prod
)
$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. ..\scripts\pack.ps1;

$projects = @(
	"Albatross.EFCore",
	"Albatross.EFCore.ChangeReporting",
	"Albatross.EFCore.DateLevel",
	"Albatross.EFCore.PostgreSQL",
	"Albatross.EFCore.SqlServer"
);

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-localSymbolServer $env:LocalSymbolServer `
	-remoteSymbolServer $env:RemoteSymbolServer `
	-nugetSource $env:DefaultNugetSource `
	-prod:$prod
