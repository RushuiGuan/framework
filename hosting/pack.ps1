param(
	[switch]
	[bool]$prod
)
$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. $PSScriptRoot\..\scripts\pack.ps1;

$projects = @(
	"Albatross.Hosting",
	"Albatross.Hosting.Test",
	"Albatross.Hosting.Utility"
);

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-remoteSymbolServer $env:RemoteSymbolServer `
	-nugetSource $env:DefaultNugetSource `
	-prod:$prod
