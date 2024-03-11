param(
	[switch]
	[bool]$prod
)
$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. $PSScriptRoot\..\scripts\pack.ps1;

$projects = @(
	"Albatross.Entity.AutoCacheEviction",
	"Albatross.Caching",
	"Albatross.Caching.Controllers",
	"Albatross.Caching.Redis",
	"Albatross.Caching.MemCache"
);

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-remoteSymbolServer $env:RemoteSymbolServer `
	-nugetSource $env:DefaultNugetSource `
	-prod:$prod
