Set-StrictMode -Version Latest;
$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";

<#
returns $true if there is any uncommitted change in the directory
#>
function Test-GitDiff {
    param(
        [Parameter(Mandatory)]
        [string]$path
    )
    if ($path) {
        [array]$check = (git status $path -s);
        return ($check.Length -ne 0);
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
