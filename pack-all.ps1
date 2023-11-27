param(
	[Parameter(Position = 0)]
	[switch]
	[bool]$prod
)
$location = Get-Location;
set-location $PSScriptRoot;

get-childitem pack.ps1 -recurse | `
	where-object { $_.FullName -notlike "*\scripts\*" } | `
	foreach-object { & $_.FullName -prod:$prod }

set-location $location;