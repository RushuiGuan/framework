import-module app-dev

$nuget_root = (Get-NugetLocal);

$config = "debug";

get-childitem $PSScriptRoot\src -r *.csproj | ForEach-Object {
	invoke-dotnetclean -csproj $_ -config $config;
	$out = Get-Path $nuget_root, $_.Name;
	Invoke-DotnetPack -csproj $_  -config $config -out $out;
}