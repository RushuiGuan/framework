$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";

. $PSScriptRoot\git.ps1;

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
        Write-Output $version;
    }
}

function Get-NewProjectVersion {
    param (
        [Parameter(Mandatory, ValueFromPipeline, Position = 0)]
        [System.IO.FileInfo]$csproj,
        [switch]
        [bool]$prod
    )
    process {
        $version = Get-ProjectVersion $csproj;
        $branch = Get-GitBranch;
        $folder = [System.IO.Path]::GetDirectoryName($csproj);
        $hash = get-GitFolderHash $folder;
        if ($prod) {
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
