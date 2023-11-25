$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. ..\scripts.ps1;

$projects = @(
	"albatross.authentication"
);

Run-Pack $projects $PSScriptRoot;