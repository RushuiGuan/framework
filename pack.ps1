param(
	[parameter(Mandatory=$true)]
	[string]
	$directory,
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
Set-StrictMode -Version Latest;

$root = "$PSScriptRoot\$directory";

if(-not [System.IO.Directory]::Exists($root)){
	Write-Error "Directory $root does not exist"
}else{
	Write-Information "Working directory: $root"
}

if(-not [System.IO.File]::Exists("$root\.projects")){
	Write-Error ".projects file not found"
}

$testProjects = devtools project-list -f "$root\.projects" -h tests
$projects = devtools project-list -f "$root\.projects" -h packages

if (-not $skipTest) {
	# run the test projects
	foreach($item in $testProjects){
		"Testing $item";
		dotnet test $root\$item\$item.csproj -c release
		if($LASTEXITCODE -ne 0){
			Write-Error "Test failed for $item"
		}
	}
}

if($projects.Length -eq 0){
	Write-Information "Nothing to do";
	return;	
}

# if $prod is true and $force is false, do not proceed if the directory is dirty
if($prod -and -not $force){
	devtools is-dirty -d $root
	if($LASTEXITCODE -ne 0){
		Write-Error "Directory is dirty. Please commit or stash changes before proceeding"
	}
}

$version = devtools project-version -d $root -p="$prod"
if($LASTEXITCODE -ne 0){
	Write-Error "Unable to get project version"
}
try {
	# first clean up the artifacts folder
	Write-Information "Cleaning up artifacts folder: $root\artifacts";
	if(-not [System.IO.Directory]::Exists("$root\artifacts")){
		New-Item -ItemType Directory -Path "$root\artifacts"
	} else {
		Get-ChildItem $root\artifacts\*.nupkg | Remove-Item -Force
	}
	Write-Information "Version: $version";
	devtools set-project-version -d $root -ver $version
	
	$repositoryProjectRoot = devtools read-project-property -f .\Directory.Build.props -p RepositoryProjectRoot
	if($LASTEXITCODE -ne 0){
		Write-Error "Unable to read RepositoryProjectRoot from the Directory.Build.props file";
	} else {
		$repositoryProjectRoot = $repositoryProjectRoot + "/README.md";
	}
	foreach($project in $projects){
		# first fix the README.md file
		$readme = "$root\$project\README.md";
		$tmp = [System.IO.Path]::GetTempFileName()
		Copy-Item $readme $tmp -Force
		try {
			if ([System.IO.File]::Exists($readme)) {
				devtools fix-markdown-relative-urls --markdown-file $readme --root-folder $PSScriptRoot --root-url $repositoryProjectRoot
				if($LASTEXITCODE -ne 0){
					Write-Error "Unable to fix the README.md file for $project"
				}
			}
			"Building $project";
			dotnet pack $root\$project\$project.csproj -c release -o $root\artifacts
			if($LASTEXITCODE -ne 0){
				Write-Error "Build failed for $project"
			}
		}finally{
			# Copy-Item $tmp $readme -Force
			# Remove-Item $tmp -Force
		}
	}
	if(-not [string]::IsNullOrEmpty($env:LocalNugetSource)){
		nuget push $root\artifacts\*.nupkg -Source $env:LocalNugetSource
	}
	if($push){
		nuget push $root\artifacts\*.nupkg -Source staging -ApiKey az;
	}
} finally {
	devtools remove-project-version -d $root
	Get-ChildItem $root\*.csproj -recurse | ForEach-Object { 
		devtools format-xml -f $_.FullName
	}
}