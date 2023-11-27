param(
	[switch]
	[bool]$prod
)
$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. ..\scripts\pack.ps1;

$projects = @(
	"Albatross.Authentication"
);

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-localSymbolServer $env:LocalSymbolServer `
	-remoteSymbolServer $env:RemoteSymbolServer `
#	-nugetSource $env:DefaultNugetSource `
	-nugetSource test `
	-prod:$prod
