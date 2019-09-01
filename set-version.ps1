import-module app-dev

Set-ProjectVersion $PSScriptRoot -version "1.5.0";


$array = @(
    "$PSScriptRoot\reflection\src\Albatross.Reflection",
    "$PSScriptRoot\crypto\src\Albatross.Cryptography",
    "$PSScriptRoot\webclient\src\Albatross.WebClient",
    "$PSScriptRoot\config\src\Albatross.Config",
    "$PSScriptRoot\mapping\src\Albatross.Mapping",
    "$PSScriptRoot\repository\src\Albatross.Repository",
    "$PSScriptRoot\authentication\src\Albatross.Authentication",
    "$PSScriptRoot\host\src\Albatross.Host.NUnit",
    "$PSScriptRoot\host\src\Albatross.Host.AspNetCore",
    "$PSScriptRoot\host\src\Albatross.Host.PowerShell"
);

$nuget_root = (Get-NugetLocal);

$config = "debug";

$array | ForEach-Object {
    invoke-dotnetclean -csproj $_ -config $config;
    $out = Get-Path $nuget_root, (get-item $_).Name;
    Invoke-DotnetPack -csproj $_  -config $config -out $out;
}