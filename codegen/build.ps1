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
		,"Albatross.CodeGen.Utility" `
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
		dotnet publish $PSScriptRoot\$project\$project.csproj -o $install\$project -c release -r win-x64
	}
}

set-alias -name codegen -Value $env:InstallDirectory\Albatross.CodeGen.Utility\codegen.exe
