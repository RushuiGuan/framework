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
		"sample.messaging.utility"
		,"sample.messaging.webapi"
		,"sample.messaging.daemon"
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
		dotnet publish $PSScriptRoot\sample\$project\$project.csproj -o $install\$project -c debug --no-restore
	}
	$project = "albatross.messaging.utility";
	"Building $project";
	dotnet publish $PSScriptRoot\src\$project\$project.csproj -o $install\$project -c debug --no-restore
}

set-alias -name sample-messaging -Value $env:InstallDirectory\sample.messaging.utility\sample.messaging.utility.exe
set-alias -name sample-messaging-webapi -Value $env:InstallDirectory\sample.messaging.webapi\sample.messaging.webapi.exe
set-alias -name sample-messaging-daemon -Value $env:InstallDirectory\sample.messaging.daemon\sample.messaging.daemon.exe
set-alias -name msg -Value $env:InstallDirectory\albatross.messaging.utility\msg.exe