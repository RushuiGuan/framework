$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. ..\scripts.ps1;

$projects = @(
	"albatross.authentication"
);

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-localSymbolServer $env:LocalSymbolServer `
	-remoteSymbolServer $env:RemoteSymbolServer;



#	-nugetSource test `
#	-nugetSource $env:DefaultNugetSource `