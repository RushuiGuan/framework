import-module app-dev

Set-ProjectVersion $PSScriptRoot -version "1.5.12";


$array = @(
    "$PSScriptRoot\authentication\src\Albatross.Authentication",
    "$PSScriptRoot\codegen\src\Albatross.CodeGen",
    "$PSScriptRoot\config\src\Albatross.Config",
    "$PSScriptRoot\crypto\src\Albatross.Cryptography",
    "$PSScriptRoot\host\src\Albatross.Host.NUnit",
    "$PSScriptRoot\host\src\Albatross.Host.AspNetCore",
    "$PSScriptRoot\host\src\Albatross.Host.PowerShell",
    "$PSScriptRoot\mapping\src\Albatross.Mapping",
    "$PSScriptRoot\reflection\src\Albatross.Reflection",
    "$PSScriptRoot\repository\src\Albatross.Repository",
    "$PSScriptRoot\repository\src\Albatross.Repository.NUnit",
    "$PSScriptRoot\webclient\src\Albatross.WebClient"
);

$nuget_root = (Get-NugetLocal);

$config = "debug";

$array | ForEach-Object {
    invoke-dotnetclean -csproj $_ -config $config;
    $out = Get-Path $nuget_root, (get-item $_).Name;
    Invoke-DotnetPack -csproj $_  -config $config -out $out;
}