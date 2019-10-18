import-module app-dev

Set-ProjectVersion $PSScriptRoot -version "1.5.20";


$nuget_root = (Get-NugetLocal);

$config = "debug";

get-childitem $PSScriptRoot\src -r *.csproj | ForEach-Object {
	invoke-dotnetclean -csproj $_.FullName -config $config;
	$out = Get-Path $nuget_root, ([System.IO.Path]::GetFileNameWithoutExtension($_.FullName));
	Invoke-DotnetPack -csproj $_.FullName  -config $config -out $out;
}