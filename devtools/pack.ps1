param(
	[switch]
	[bool]$skipTest,
	[switch]
	[bool]$prod,
	[switch]
	[bool]$force,
	[switch]
	[bool]$push
)

$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
Set-StrictMode -Version Latest


$projects = @(
	"Albatross.DevTools",
	"Albatross.SemVer"
);

$testProjects = @(
	"Albatross.DevTools.UnitTest",
	"Albatross.SemVer.UnitTest"
);

if (-not $skipTest) {
	# run the test projects
	foreach($item in $testProjects){
		"Testing $item";
		dotnet test $PSScriptRoot\$item\$item.csproj -c release
		if($LASTEXITCODE -ne 0){
			Write-Error "Test failed for $item"
		}
	}
}

# if $prod is true and $force is false, do not proceed if the directory is dirty
if($prod -and -not $force){
	devtools is-dirty -d $PSScriptRoot
	if($LASTEXITCODE -ne 0){
		Write-Error "Directory is dirty. Please commit or stash changes before proceeding"
	}
}

$version = devtools project-version -d $PSScriptRoot -p="$prod"
if($LASTEXITCODE -ne 0){
	Write-Error "Unable to get project version"
}

try {
	# first clean up the artifacts folder
	Get-ChildItem $PSScriptRoot\artifacts\*.nupkg | Remove-Item -Force

	Write-Information "Version: $version";
	devtools set-project-version -d $PSScriptRoot -ver $version

	foreach($project in $projects){
		"Building $project";
		dotnet pack $PSScriptRoot\$project\$project.csproj -c release -o $PSScriptRoot\artifacts
		if($LASTEXITCODE -ne 0){
			Write-Error "Build failed for $project"
		}
	}
	nuget push $PSScriptRoot\artifacts\*.nupkg -Source $env:LocalNugetSource
	if($push){
		nuget push $PSScriptRoot\artifacts\*.nupkg -Source staging;
	}
} finally {
	devtools remove-project-version -d $PSScriptRoot
	Get-ChildItem *.csproj -recurse | ForEach-Object { 
		devtools format-xml -f $_.FullName
	}
}