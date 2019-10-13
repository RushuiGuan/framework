@{

	# RootModule = ''
	ModuleVersion     = '0.0.1'
	GUID              = 'c42d1047-db8b-411e-a622-f70ce216eec5'
	Author            = 'rushui'
	CompanyName       = 'Unknown'
	Copyright         = '(c) rushui. All rights reserved.'
	# Description = ''
	# RequiredAssemblies = @()
	NestedModules     = @(".\Albatross.Cryptography.PowerShell.dll")
	FunctionsToExport = @()
	CmdletsToExport   = @("Get-RandomBytes", "Get-SHAHash", "Get-HMACShaHash", "Convert-Base64ToHex")
	VariablesToExport = '*'
	AliasesToExport   = @()

}

