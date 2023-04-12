$output = "$env:InstallDirectory";

Get-ChildItem $output\Albatross.Caching.Utility | Remove-Item -Force -Recurse;
Get-ChildItem $output\Albatross.Framework.Utility | Remove-Item -Force -Recurse;

dotnet publish $PSScriptRoot\src\Albatross.Caching.Utility\Albatross.Caching.Utility.csproj -c release -o $output\Albatross.Caching.Utility
dotnet publish $PSScriptRoot\tests\Albatross.Framework.Utility\Albatross.Framework.Utility.csproj -c release -o $output\Albatross.Framework.Utility
