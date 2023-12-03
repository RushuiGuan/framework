$InformationPreference = "Continue";
$install = $env:InstallDirectory;


$projects = @(
	"sample.logging.utility"
	,"sample.logging.webapi"
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
	dotnet publish $PSScriptRoot\$project\$project.csproj -o $install\$project -c debug
}

set-alias -name sample-logging -Value $env:InstallDirectory\sample.logging.utility\sample.logging.utility.exe
set-alias -name sample-logging-api -Value $env:InstallDirectory\sample.logging.webapi\sample.logging.webapi.exe
