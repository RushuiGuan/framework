$location = Get-Location;
Set-Location $PSScriptRoot;

dotnet run exec-script --location PreMigrationScripts;
dotnet run ef-migrate
dotnet run create-sql-script --output-file .\db\tables.sql --drop-script .\db\drop.sql
dotnet run exec-script
Set-Location $location;
