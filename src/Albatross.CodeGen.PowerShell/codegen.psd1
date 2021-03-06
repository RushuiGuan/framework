@{
    # RootModule = ''
    ModuleVersion      = '1.0'
    GUID               = 'd7672051-e1d2-46bc-a380-0629146936f6'
    Author             = 'rushui'
    CompanyName        = 'Unknown'
    Copyright          = '(c) 2019 rushui. All rights reserved.'
    RequiredAssemblies = @()
    NestedModules      = @('.\Albatross.CodeGen.PowerShell.dll')
    FunctionsToExport  = @()
    CmdletsToExport    = @(
        "Invoke-CSharpClassGenerator", "Invoke-TypeScriptClassGenerator", "Read-JsonObject",
        "Get-DatabaseTable", "Get-StoredProcedure", "New-DatabaseServer", "New-DatabaseTable",
        "Invoke-ClassLoader"
    )
    VariablesToExport  = '*'
    AliasesToExport    = @()
}