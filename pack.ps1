import-module app-dev

$config = "release";

get-childitem $PSScriptRoot\src\*.csproj -r | ForEach-Object {
	invoke-dotnetclean -csproj $_ -config $config;
	Invoke-DotnetPack -csproj $_  -config $config -out $PSScriptRoot\bin;
}