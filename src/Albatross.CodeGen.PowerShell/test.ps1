$path = "$PSScriptRoot\bin"
Get-ChildItem $path | Remove-Item -Recurse -Force;
dotnet publish $PSScriptRoot\Albatross.CodeGen.PowerShell.csproj --output $path
import-module $PSScriptRoot\bin\Albatross.CodeGen.PowerShell.dll

$import = new-object -type Albatross.CodeGen.TypeScript.Model.Import
$import.name = ""