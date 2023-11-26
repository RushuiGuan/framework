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
		[string]$localSymbolServer,
		[string]$remoteSymbolServer,
		[switch]
		[bool]$prod
	)
	process {
		if (Test-GitDiff $directory) {
			Write-Error "Please commit your changes before packing";
		}
		$install = "$directory\artifacts";
		$name = (Get-Item $directory).Name.ToLower();
		$versionFile = "$directory\Directory.Build.props";
		if (-not (Test-Path $versionFile -type Leaf)) {
			Write-Error "Missing Directory.Build.props file";
		}
		foreach ($project in $projects) {
			Write-Information "Deleting existing artifacts for $project";
			Set-Directory "$install\$item";
			Get-ChildItem $install\$item | Remove-Item -Recurse -Force;
		}
		$currentVersion = Get-ProjectVersion $versionFile;
		$version = Get-NewProjectVersion $versionFile -prod:$prod;
		Set-ProjectVersion -csproj $versionFile -version $version;
		Write-Information "Project version updated to: $version";
		dotnet restore $directory;
		foreach ($project in $projects) {
			Write-Information "Building $project";
			dotnet pack $directory\$project\$project.csproj `
				--output $install\$project `
				--configuration release `
				--no-restore
		}
		Write-Information "Reverting version file";
		git checkout $versionFile;

		$hasNugetPush = $false;
		foreach ($project in $projects) {
			if ($nugetSource) { 
				get-item $install\$project\*.nupkg | foreach-Object {
					nuget push $_.FullName -source $nugetSource -apiKey az
					if ($exitcode -ne 0){
					#	Write-Error "Error push nuget package $($_.Name)";
					}
					$hasNugetPush = $true;
				}
			}
			try {
				if ($localSymbolServer) {
					$path = (Set-Directory "$localSymbolServer\$project\")
					write-Information "copy local symbol => $path";
					copy-item $install\$project\*.snupkg $path;
				}
				if ($remoteSymbolServer -and (Test-Path $remoteSymbolServer -type Container)) {
					$path = (Set-Directory "$remoteSymbolServer\$project")
					write-Information "copy remote symbol => $path";
					copy-item $install\$project\*.snupkg $path;
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