param([string]$name)

if ([string]::IsNullOrEmpty($name)) {
    Write-Error "input parameter name is required!";
    return;
}


dotnet ef migrations add CRMSqlMigration_$name --context CRMDbSqlMigrationSession --output-dir Migrations\CRMSqlServer
dotnet ef migrations add CRMPostgresMigration_$name --context CRMDbPostgresMigrationSession --output-dir Migrations\CRMPostgres
dotnet ef migrations add CRMSqliteMigration_$name --context CRMDbSqlLiteMigrationSession --output-dir Migrations\CRMSqlite

# dotnet ef migrations add IdentityServerConfigSqlServerMigration_$name --context IdentityServerConfigSqlServerMigration --output-dir Migrations\ConfigSqlServer
# dotnet ef migrations add IdentityServerConfigPostgreSQLMigration_$name --context IdentityServerConfigPostgreSQLMigration --output-dir Migrations\ConfigPostgres

# dotnet ef migrations add IdentityServerPersistedGrantSqlServerMigration_$name --context IdentityServerPersistedGrantSqlServerMigration --output-dir Migrations\PersistedGrantSqlServer
# dotnet ef migrations add IdentityServerPersistedGrantPostgreSQLMigration_$name --context IdentityServerPersistedGrantPostgreSQLMigration --output-dir Migrations\PersistedGrantPostgres
