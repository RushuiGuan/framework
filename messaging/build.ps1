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
		"albatross.messaging.utility",
		"sample.utility"
		,"sample.webapi"
		,"sample.daemon"
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

set-alias -name sample-messaging -Value $env:InstallDirectory\sample.utility\sample.utility.exe
set-alias -name sample-messaging-webapi -Value $env:InstallDirectory\sample.webapi\sample.webapi.exe
set-alias -name sample-messaging-daemon -Value $env:InstallDirectory\sample.daemon\sample.daemon.exe
set-alias -name msg -Value $env:InstallDirectory\albatross.messaging.utility\msg.exe