param(
	[switch]
	[bool]$alias
)
$InformationPreference = "Continue";
$install = $env:InstallDirectory;

if(-not $alias) {
	$projects = @(
		"sample.hosting.utility"
		,"sample.hosting.webapi"
		,"sample.hosting.daemon"
	);

	foreach($item in $projects){
		if(Test-Path "$install/$item" -type Container){
			Write-Information "Deleting $item";
 			Get-ChildItem $install\$item | Remove-Item -Recurse -Force;
		}
	}

	dotnet restore $PSScriptRoot --interactive;
	foreach($project in $projects){
		"Building $project";
		dotnet publish $PSScriptRoot\$project\$project.csproj -o $install\$project -c debug --interactive;
	}
}

set-alias -name sample-hosting -Value $env:InstallDirectory\sample.hosting.utility\sample.hosting.utility.exe
set-alias -name sample-hosting-webapi -Value $env:InstallDirectory\sample.hosting.webapi\sample.hosting.webapi.exe
set-alias -name sample-hosting-daemon -Value $env:InstallDirectory\sample.hosting.daemon\sample.hosting.daemon.exe
