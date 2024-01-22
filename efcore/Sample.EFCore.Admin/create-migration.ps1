$location = Get-Location;
Set-Location $PSScriptRoot;

if(test-Path $PSScriptRoot\Migrations){
	$count = (Get-Childitem $PSScriptRoot\Migrations\ -recurse -filter *_SampleSqlServerMigration_v*.Designer.cs  | Measure-Object).Count + 1;
}else{
	$count = 1;
}

$name = "v$count";
"Creating migration $name";
dotnet ef migrations add SampleSqlServerMigration_$name --context SampleSqlServerMigration --output-dir (Get-Path Migrations, SqlServer)

"PreMigrationScripts";
dotnet run exec-script --location PreMigrationScripts;

"Migrate";
dotnet run ef-migrate

"PostMigrationScripts";
dotnet run exec-script

"Generate scripts"
dotnet run create-sql-script --output-file .\db\tables.sql --drop-script .\db\drop.sql

Set-Location $location;
