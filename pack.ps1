import-module app-dev

$nuget_root = (Get-NugetLocal);

$config = "debug";

get-childitem $PSScriptRoot\src\*.csproj -r | ForEach-Object {
	invoke-dotnetclean -csproj $_ -config $config;
	Invoke-DotnetPack -csproj $_  -config $config -out $nuget_root;
}