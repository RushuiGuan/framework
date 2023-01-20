$location = Get-Location;

Set-Location $PSScriptRoot;

if(test-Path $PSScriptRoot\Migrations){
	$count = (Get-Childitem $PSScriptRoot\Migrations\ -recurse -filter *_MySqlServerMigration_v*.Designer.cs  | Measure-Object).Count + 1;
}else{
	$count = 1;
}

$name = "v$count";

"Creating migration $name";

dotnet ef migrations add MySqlServerMigration_$name --context MySqlServerMigration --output-dir (Get-Path Migrations, SqlServer)
Set-Location $location;
