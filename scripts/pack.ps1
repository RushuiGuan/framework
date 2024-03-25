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
		[switch]
		[bool]$prod,
		[switch]
		[bool]$force
	)
	process {
		if ((-not $force) -and (Test-GitDiff $directory)) {
			Write-Error "Please commit your changes before packing";
		}
		dotnet restore $directory --verbosity minimal
		
		$artifacts = "$directory\artifacts";
		Write-Information "Cleaning artifacts directory: $artifacts"
		Get-ChildItem $artifacts | foreach-object { Remove-Item $_.FullName; }

		$versionFile = "$directory\Directory.Build.props";
		if (-not (Test-Path $versionFile -type Leaf)) {
			Write-Error "Missing Directory.Build.props file";
		}
		$currentVersion = Get-ProjectVersion $versionFile;
		$version = Get-NewProjectVersion $versionFile -prod:$prod;
		Set-ProjectVersion -csproj $versionFile -version $version;
		Write-Information "Project version updated to: $version";
		foreach ($project in $projects) {
			Write-Information "Building $project";
			dotnet pack $directory\$project `
				--output $artifacts `
				--configuration release `
				--no-restore
		}
		Write-Information "Reverting version file";
		git checkout $versionFile;

		$hasNugetPush = $false;
		if ($nugetSource) { 
			get-item $artifacts\*.nupkg | foreach-Object {
				nuget push $_.FullName -source $nugetSource -apiKey az
				if ($exitcode -ne 0){
				#	Write-Error "Error push nuget package $($_.Name)";
				}
				$hasNugetPush = $true;
			}
		}
		if ($hasNugetPush -and $prod) {
			$tag = "prod-$currentVersion";
			Write-Information "Tagging: $tag";
			git tag $tag;
		}
	}
}