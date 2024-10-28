param(
	[Parameter(Position=0)]
	[string]$project,
	[switch]
	[bool]$alias
)
$InformationPreference = "Continue";
$install = $env:InstallDirectory;

if(-not $alias) {
	$projects = @(
		"Sample.EFCore.Admin"
	);

	if(-not [string]::IsNullOrEmpty($project)){
		$projects = $projects | Where-Object { $_ -like "*$project" }
	}

	foreach($item in $projects){
		if(Test-Path "$install/$item" -type Container){
			Write-Information "Deleting $item";
 			Get-ChildItem $install\$item | Remove-Item -Recurse -Force;
		}
	}

	dotnet restore $PSScriptRoot
	foreach($project in $projects){
		"Building $project";
		dotnet publish $PSScriptRoot\$project\$project.csproj -o $install\$project -c debug --no-restore
	}
}

set-alias -name sample-efcore-admin -Value $env:InstallDirectory\Sample.EFCore.Admin\Sample.EFCore.Admin.exe