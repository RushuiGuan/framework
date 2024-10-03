Set-StrictMode -Version Latest;
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

function Replace-ProjectReference {
	param (
		[Parameter(Mandatory, ValueFromPipeline, Position = 0)]
		[System.IO.FileInfo]$csproj,
		[Parameter(Mandatory, Position = 1)]
		[string]$match,
		[Parameter(Mandatory, Position = 2)]
		[string]$version
	)
	process {
		$file = Get-Item $csproj;
		[xml]$doc = Get-Content $file.FullName;
		$elements = Select-Xml -Xml $doc -XPath "//Project/ItemGroup/ProjectReference[contains(@Include, $match)]" 
		foreach ($element in $elements) {
			$path = $element.Node.Attributes.Where({ $_.Name -eq "Include" }).Value;

			if ($path -match "([a-z0-9_\.]+)\.csproj") {
				$project = $matches[1];
				$parent = $element.Node.ParentNode;
				$parent.RemoveChild($element.Node) | Out-Null;
				$path = $element.Node.Attributes.Where({ $_.Name -eq "Include" }).Value;
				$newNode = $doc.CreateElement("PackageReference");
				$newNode.SetAttribute("Include", $project);
				$newNode.SetAttribute("Version", $version);
				foreach ($child in $element.Node.ChildNodes) {
					$newNode.AppendChild($child.CloneNode($true)) | Out-Null;
				}
				foreach ($attrib in $element.Node.Attributes) {
					if ($attrib.Name -ne "Include") {
						$newNode.SetAttribute($attrib.Name, $attrib.Value);
					}
				}
				$parent.AppendChild($newNode) | Out-Null;
			}
		}
		$doc.Save($file.FullName);
	}
}

function Remove-PackageReference {
	param (
		[Parameter(Mandatory, ValueFromPipeline, Position = 0)]
		[System.IO.FileInfo]$csproj,
		[Parameter(Mandatory, Position = 1)]
		[string]$packageId
	)
	process {
		$file = Get-Item $csproj;
		[xml]$doc = Get-Content $file.FullName;
		
		$elements = Select-Xml -Xml $doc -XPath "/Project/ItemGroup/PackageReference[@Include='$packageId']"
		foreach ($element in $elements) {
			$parent = $element.Node.ParentNode;
			$parent.RemoveChild($element.Node) | Out-Null;
		}
		$doc.Save($file.FullName);
	}
}