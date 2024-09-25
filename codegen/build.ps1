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
		"Albatross.CodeGen.CommandLine"
		` "Test.WebApi"
		` "Test.CommandLine"
		` "Test.CodeGen"
	);


	$serviceProjects =@(
		"Test.WebApi"
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

	set-alias -name codegen -Value $env:InstallDirectory\Albatross.CodeGen.CommandLine\codegen.exe
	set-alias -name proxy-test -Value $env:InstallDirectory\Test.CommandLine\test.commandline.exe
	set-alias -name proxy-test-legacy -Value $env:InstallDirectory\Test.CodeGen\test.codegen.exe