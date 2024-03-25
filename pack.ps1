param(
	[Parameter(Position=0)]
	[string]$project,
	[switch]
	[bool]$prod
)
$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. $PSScriptRoot\scripts\pack.ps1;

$projects = @(
	"Albatross.Math"
);

if(-not [string]::IsNullOrEmpty($project)){
	$projects = $projects | Where-Object { $_ -like "*$project" }
}

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-nugetSource $env:DefaultNugetSource `
	-prod:$prod
