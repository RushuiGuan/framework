param(
	[switch]
	[bool]$prod
)
$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. $PSScriptRoot\..\scripts\pack.ps1;

$projects = @(
	"Albatross.EFCore",
	"Albatross.EFCore.ChangeReporting",
	"Albatross.EFCore.AutoCacheEviction",
	"Albatross.EFCore.Audit",
	"Albatross.DateLevel",
	"Albatross.EFCore.PostgreSQL",
	"Albatross.EFCore.SqlServer"
);

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-nugetSource $env:DefaultNugetSource `
	-prod:$prod
