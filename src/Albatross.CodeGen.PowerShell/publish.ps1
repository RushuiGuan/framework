$path = "C:\dev-tools\modules\codegen"
Get-ChildItem $path | Remove-Item -Recurse -Force;
dotnet publish $PSScriptRoot\Albatross.CodeGen.PowerShell.csproj --output $path