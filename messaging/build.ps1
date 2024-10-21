param(
	[Parameter(Position=0)]
	[string]$project,
	[switch][bool]$skip,
	[switch][bool]$run
)
$InformationPreference = "Continue";
$install = $env:InstallDirectory;

function StopProcessAndWait($processName){
	$process = Get-Process -Name $processName -ErrorAction Ignore
	if($process){
		Write-Information "Stopping process $processName";
		do{
			Stop-Process -Name $processName -Force -ErrorAction Ignore;
			Start-Sleep -Seconds 1;
		}while(Get-Process -Name $processName -ErrorAction Ignore)
		Write-Output $processName;
	}
}

	$projects = @(
		"Albatross.Messaging.Utility"
		` "Sample.WebApi"
		` "Sample.Utility"
		` "Sample.Daemon"
	);


	$serviceProjects =@(
		"Sample.WebApi"	
		` "Sample.Daemon"
	);

	$autoRestartProjects = @();	

	if(-not [string]::IsNullOrEmpty($project)){
		$projects = $projects | Where-Object { $_ -like "*$project" }
	}

	if(-not $skip) {
		foreach($item in $projects){
			if($serviceProjects -contains $item){
				StopProcessAndWait $item | ForEach-Object { $autoRestartProjects += $_ }
			}

			if(Test-Path "$install/$item" -type Container){
				Write-Information "Deleting $item";
 				Get-ChildItem $install\$item | Remove-Item -Recurse -Force;
			}
		}

		dotnet restore $PSScriptRoot
		foreach($project in $projects){
			dotnet publish $PSScriptRoot\$project\$project.csproj -o $install\$project -c release
		}
	}

	$projects2Run = @();
	if($run){
		$projects2Run = $serviceProjects;
	}else{
		$projects2Run = $autoRestartProjects;
	}

	if($projects2Run.Length -gt 0){
		$hasTab = $false;
		$projects2Run | ForEach-Object {
			Write-Information "Starting $_";
			if($hasTab){
				wt -w 0 split-pane --title $_ -d $PSScriptRoot $env:InstallDirectory\$_\$_.exe
			}else{
				wt -w 0 new-tab --title $_ -d $PSScriptRoot $env:InstallDirectory\$_\$_.exe
				$hasTab = $true;
			}
		}
	}

set-alias -name sample -Value $env:InstallDirectory\sample.utility\sample.exe
set-alias -name sample-messaging-webapi -Value $env:InstallDirectory\sample.webapi\sample.webapi.exe
set-alias -name sample-messaging-daemon -Value $env:InstallDirectory\sample.daemon\sample.daemon.exe
set-alias -name msg -Value $env:InstallDirectory\albatross.messaging.utility\msg.exe