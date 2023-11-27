$location = Get-Location;
set-location $PSScriptRoot;
get-childitem pack.ps1 -recurse | foreach-object { & $_.FullName }
set-location $location;