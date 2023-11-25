$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. ..\scripts.ps1;

if(Test-GitDiff){

}

$install = "$PSScriptRoot\artifacts";

write-Information "Install path: $install";

$projects = @(
	"albatross.authentication"
);

foreach($item in $projects){
	if(Test-Path "$install/$item" -type Container){
		Write-Information "Deleting $item";
 		Get-ChildItem $install\$item | Remove-Item -Recurse -Force;
	}
}

dotnet restore  $PSScriptRoot
foreach($project in $projects){
	"Building $project";
	dotnet pack $PSScriptRoot\$project\$project.csproj --output $install\$project --configuration release --no-restore
}
