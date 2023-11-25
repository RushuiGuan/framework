$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. ..\scripts.ps1;

$projects = @(
	"albatross.authentication"
);

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-nugetSource $env:DefaultNugetSource `
	-localSymbolServer $env:LocalSymbolServer `
	-remoteSymbolServer $env:RemoteSymbolServer;