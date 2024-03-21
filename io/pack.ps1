param(
	[switch]
	[bool]$prod
)
$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. $PSScriptRoot\..\scripts\pack.ps1;

$projects = @(
	"Albatross.IO"
);

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-nugetSource $env:DefaultNugetSource `
	-prod:$prod