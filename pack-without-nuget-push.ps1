param(
	[Parameter(Position = 0)]
	[switch]
	[bool]$prod
)
$location = Get-Location;
set-location $PSScriptRoot;
$env:DefaultNugetSource = $null;
get-childitem pack.ps1 -recurse | foreach-object { & $_.FullName }
set-location $location;