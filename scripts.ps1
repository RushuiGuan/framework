$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";

function Set-Directory {
    param(
        [parameter(Mandatory, Position = 0)]
        [string]$path
    )

    [System.IO.DirectoryInfo]$folder = New-Object System.IO.DirectoryInfo -ArgumentList $path;

    if (-not $folder.Exists) {
        $stack = New-Object System.Collections.Stack;
        do {
            $stack.Push($folder);
            $folder = $folder.Parent;
        }while (!$folder.Exists)

        while ($stack.Count -gt 0) {
            New-Item $stack.Pop().FullName -ItemType Container > $null;
        }
    }
    return $path;
}

<#
returns $true if there is any uncommitted change in the directory
#>
function Test-GitDiff {
    param(
        [Parameter(Mandatory)]
        [string]$path
    )
    if ($path) {
        $check = (git status $path -s);
        return ($check.Count -ne 0);
    }
    else {
        return $false;
    }
}
function Get-GitCount {
    param(
        [Parameter(Mandatory)]
        [string]$path
    )
    return (git rev-list --count HEAD -- $path);
}

function Get-GitBranch {
    return (git rev-parse --abbrev-ref HEAD);
}

function Get-GitHash {
    return (git rev-parse --short HEAD)
}

function Get-GitFolderHash {
    param(
        [Parameter(Mandatory)]
        [string]$path
    )
    return (git log --pretty=tformat:"%h" -n1 $path);
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
        $folder = [System.IO.Path]::GetDirectoryName($csproj);
        $hash = get-GitFolderHash $folder;
        if ($branch -eq "production") {
            write-Output "$version+$hash";
        }
        else {
            $count = Get-GitCount $folder;
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

function Run-Pack {
    param(
        [Parameter(Mandatory)]
        [string[]]$projects,
        [Parameter(Mandatory)]
        [string]$directory,
        [string]$nugetSource,
        [string]$localSymbolServer,
        [string]$remoteSymbolServer
    )
    process {
        if (Test-GitDiff $directory) {
            Write-Error "Please commit your changes before packing";
        }
        $install = "$directory\artifacts";
        $versionFile = "$directory\Directory.Build.props";
        if (-not (Test-Path $versionFile -type Leaf)) {
            Write-Error "Missing Directory.Build.props file";
        }
        foreach ($project in $projects) {
            Write-Information "Deleting existing artifacts for $project";
            Get-ChildItem $install\$item | Remove-Item -Recurse -Force;
        }
        $version = Get-ProjectVersion $versionFile;
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

        foreach ($project in $projects) {
            if ($nugetSource) { 
                get-item $install\$project\*.nupkg | foreach-Object {
                    nuget push $_.FullName -source $nugetSource -apiKey az
                }
            }

            
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
    }
}