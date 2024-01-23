param(
	[switch]
	[bool]$prod
)
$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";

. $PSScriptRoot\..\scripts\pack.ps1;

$projects = @(
	"Albatross.Authentication",
	"Albatross.Authentication.Windows",
	"Albatross.Authentication.AspNetCore"
);

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-remoteSymbolServer $env:RemoteSymbolServer `
	-nugetSource $env:DefaultNugetSource `
	-prod:$prod
