$output = "$env:InstallDirectory";

Get-ChildItem $output\Albatross.Caching.Utility | Remove-Item -Force -Recurse;
dotnet publish $PSScriptRoot\src\Albatross.Caching.Utility\Albatross.Caching.Utility.csproj -c release -o $output\Albatross.Caching.Utility
