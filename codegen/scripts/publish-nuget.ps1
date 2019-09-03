$ErrorActionPreference = "stop";
Import-Module app-dev;
$app_root = Get-Item $PSScriptRoot\..;

Remove-NugetCache Albatross.CodeGen*;

$array = @(
	"$app_root\src\Albatross.CodeGen"
);

$local = (Get-NugetLocal);
$config = "debug";

$array | ForEach-Object {
	Invoke-DotnetClean -csproj $_ -config $config;
	$out = Get-Path $local, (get-item $_).Name;
	Invoke-DotnetPack -csproj $_  -config $config -out $out;
}