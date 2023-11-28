$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";

. $PSScriptRoot\git.ps1;
. $PSScriptRoot\io.ps1;
. $PSScriptRoot\csproj.ps1;
function Run-Pack {
	param(
		[Parameter(Mandatory)]
		[string]$directory,
		[Parameter(Mandatory)]
		[string[]]$projects,
		[string]$nugetSource,
		[string]$remoteSymbolServer,
		[switch]
		[bool]$prod
	)
	process {
		if (Test-GitDiff $directory) {
			Write-Error "Please commit your changes before packing";
		}
		$artifacts = "$directory\artifacts";
		$name = (Get-Item $directory).Name.ToLower();
		$versionFile = "$directory\Directory.Build.props";
		if (-not (Test-Path $versionFile -type Leaf)) {
			Write-Error "Missing Directory.Build.props file";
		}
		foreach ($project in $projects) {
			Write-Information "Deleting existing artifacts for $project";
			Set-Directory "$artifacts\$project";
			Get-ChildItem $artifacts\$project | Remove-Item -Recurse -Force;
		}
		$currentVersion = Get-ProjectVersion $versionFile;
		$version = Get-NewProjectVersion $versionFile -prod:$prod;
		Set-ProjectVersion -csproj $versionFile -version $version;
		Write-Information "Project version updated to: $version";
		dotnet restore $directory --interactive;
		foreach ($project in $projects) {
			Write-Information "Building $project";
			dotnet pack $directory\$project\$project.csproj `
				--output $artifacts\$project `
				--configuration release `
				--no-restore
		}
		Write-Information "Reverting version file";
		git checkout $versionFile;

		$hasNugetPush = $false;
		foreach ($project in $projects) {
			if ($nugetSource) { 
				get-item $artifacts\$project\*.nupkg | foreach-Object {
					nuget push $_.FullName -source $nugetSource -apiKey az
					if ($exitcode -ne 0){
					#	Write-Error "Error push nuget package $($_.Name)";
					}
					$hasNugetPush = $true;
				}
			}
			try {
				if ($remoteSymbolServer -and (Test-Path $remoteSymbolServer -type Container)) {
					$path = (Set-Directory $remoteSymbolServer)
					write-Information "copy remote symbol for $project => $path";
					copy-item -Path $artifacts\$project\*.snupkg -Destination $path -Verbose;
				}
			}
			catch {}
		}
		if ($hasNugetPush -and $prod) {
			$tag = "$name-$currentVersion";
			Write-Information "Tagging: $tag";
			git tag $tag;
		}
	}
}