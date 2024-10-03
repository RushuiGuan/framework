Set-StrictMode -Version Latest;
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
