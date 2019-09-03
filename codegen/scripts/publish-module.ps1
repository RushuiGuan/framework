$ErrorActionPreference = "stop";
Import-Module app-dev;
$app_root = Get-Item $PSScriptRoot\..;

$dst = get-path $app_root, dist, modules, codegen;
Invoke-DotnetPublish -csproj $app_root\src\Albatross.CodeGen.PowerShell -out $dst -runtime win-x64 -config release -framework netcoreapp2.2;
Copy-Item $app_root\scripts\codegen.psd1 $dst -Force;