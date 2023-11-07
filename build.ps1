$output = $env:InstallDirectory;

$projects = @(
	"sampleproject.utility"
	,"sampleproject.daemon"
	,"sampleproject.webapi"
);

foreach($project in $projects){
	Get-ChildItem $output\$project | Remove-Item -Recurse -Force;
}

dotnet restore
foreach($project in $projects){
	dotnet publish $PSScriptRoot\samples\$project\$project.csproj -o $output\$project -c debug --no-restore
}

