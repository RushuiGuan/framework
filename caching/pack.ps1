param(
	[switch]
	[bool]$prod
)
$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. $PSScriptRoot\..\scripts\pack.ps1;

$projects = @(
	"Albatross.EFCore.AutoCacheEviction",
	"Albatross.Caching",
	"Albatross.Caching.Controllers",
	"Albatross.Caching.Redis",
	"Albatross.Caching.MemCache"
);

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-nugetSource $env:DefaultNugetSource `
	-prod:$prod
