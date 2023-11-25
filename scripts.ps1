$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";

<#
returns $true if there is any uncommitted change in the directory
#>
function Test-GitDiff ([string]$path) {
	if ($path) {
		$check = (git status $path -s);
		return ($check.Count -ne 0);
	}
	else {
		return $false;
	}
}
function Get-GitCount {
    return (git rev-list --count HEAD);
}

function Get-GitBranch(){
	return (git rev-parse --abbrev-ref HEAD);
}

function Get-GitHash(){
	return (git rev-parse --short HEAD)
}

function Get-LastModifiedGitHash([string]$path){

}

function Get-ProjectVersion {
    param (
        [Parameter(Mandatory, ValueFromPipeline, Position = 0)]
        [System.IO.FileInfo]$csproj
    )
    process {
        [string]$xpath = "/Project/PropertyGroup/Version";
        $node = (Select-Xml -Path $csproj.FullName -XPath $xpath);
        [string]$version = $null;
        if ($node) {
            $version = $node.Node.InnerText;
        }
        else {
            $version = "1.0.0";
        }
        $branch = Get-GitBranch;
        $hash = get-GitHash
        if($branch -eq "production"){
            write-Output "$version+$hash";
        }else{
            $count = Get-GitCount;
            write-Output "$version-$count.$branch+$hash";
        }
    }
}

# Set the version of a csproj file
function Set-ProjectVersion {
    param(
        [Parameter(Mandatory, ValueFromPipeline, Position = 0)]
        [System.IO.FileInfo]$csproj,
        [Parameter(Mandatory, Position = 1)]
        [string]$version
    )
    process { 
        [xml]$doc = Get-Content $csproj.FullName;
        Set-XmlElementValue -doc $doc -names Project, PropertyGroup, Version -value $version;
        $doc.Save($csproj.FullName);
        Write-Output $csproj;
    }
}

function Run-Pack{
    param(
        [Parameter(Mandatory)]
        [string[]]$projects,
        [Parameter(Mandatory)]
        [string]$directory
    )
    process{
        if(Test-GitDiff $directory){
            Write-Error "Please commit your changes before packing";
        }
        $install = "$directory\artifacts";
        $versionFile = "$directory\Directory.Build.props";
        if(-not (Test-Path $versionFile -type Leaf)){
            Write-Error "Missing Directory.Build.props file";
        }
        foreach($project in $projects){
            Write-Information "Deleting existing artifacts for $project";
            Get-ChildItem $install\$item | Remove-Item -Recurse -Force;
        }
        $version = Get-ProjectVersion $versionFile;
        Set-ProjectVersion -csproj $versionFile -version $version;
        dotnet restore $directory;
        foreach($project in $projects){
            Write-Information "Building $project";
            dotnet pack $directory\$project\$project.csproj `
                --output $install\$project `
                --configuration release `
                --no-restore
        }
        git checkout $versionFile;
    }
}