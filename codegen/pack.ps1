param(
	[switch]
	[bool]$prod
)
$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. $PSScriptRoot\..\scripts\pack.ps1;

$projects = @(
	"Albatross.CodeGen",
	"Albatross.CodeGen.CSharp",
	"Albatross.CodeGen.Python",
	"Albatross.CodeGen.TypeScript",
	"Albatross.CodeGen.WebClient"
);

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-remoteSymbolServer $env:RemoteSymbolServer `
	-nugetSource $env:DefaultNugetSource `
	-prod:$prod
