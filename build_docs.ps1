# if docfx has not been installed, run below
# dotnet tool update -g docfx --ignore-failed-sources


Get-ChildItem $PSScriptRoot\docs | Remove-Item -Recurse -Force;

Set-Location $PSScriptRoot\docfx_project
docfx metadata
docfx build
Set-Location $PSScriptRoot