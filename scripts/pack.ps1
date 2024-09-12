$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";

. $PSScriptRoot\git.ps1;
. $PSScriptRoot\io.ps1;
. $PSScriptRoot\csproj.ps1;

function Update-CodeGenProjectReference {
	param (
		[Parameter(Mandatory)]
		[string]$csproj,
		[Parameter(Mandatory)]
		[string]$version
	)

	process { 
		[xml]$doc = Get-Content $csproj;
		# find the project reference to Albatross.CodeAnalysis.csproj and remove it
		$node = Select-Xml -Xml $doc -XPath '//Project/ItemGroup/ProjectReference[contains(@Include, "Albatross.CodeAnalysis.csproj")]' | Select-Object -ExpandProperty Node
		if ($node) {
			$parentNode = $node.ParentNode
			$parentNode.RemoveChild($node) | Out-Null
		}

		# find the package reference to Albatross.CodeAnalysis and remove it
		$node = Select-Xml -Xml $doc -XPath '//Project/ItemGroup/PackageReference[@Include="Albatross.CodeAnalysis"]' | Select-Object -ExpandProperty Node
		if ($node) {
			$parentNode = $node.ParentNode
			$parentNode.RemoveChild($node) | Out-Null
		}

		# create the package reference to Albatross.CodeAnalysis with the new version
		$node = Select-Xml -Xml $doc -XPath '//Project/ItemGroup' | Select-Object -ExpandProperty Node
		$newElement = $doc.CreateElement("PackageReference")
		$newElement.SetAttribute("Include", 'Albatross.CodeAnalysis')
		$newElement.SetAttribute("Version", $version)
		$newElement.SetAttribute("GeneratePathProperty", 'true')
		$newElement.SetAttribute("PrivateAssets", 'all')
		$node.AppendChild($newElement) | Out-Null

		# create the TargetPathWithTargetPlatformMoniker element for Albatross.CodeAnalysis.dll if it doesn't exist
		$node = Select-Xml -Xml $doc -XPath '//Project/Target/ItemGroup/TargetPathWithTargetPlatformMoniker[@Include="$(PKGAlbatross_CodeAnalysis)\lib\netstandard2.0\Albatross.CodeAnalysis.dll"]' | Select-Object -ExpandProperty Node
		if (-not $node) {
			$node = Select-Xml -Xml $doc -XPath '//Project/Target/ItemGroup' | Select-Object -ExpandProperty Node
			$newElement = $doc.CreateElement("TargetPathWithTargetPlatformMoniker")
			$newElement.SetAttribute("Include", '$(PKGAlbatross_CodeAnalysis)\lib\netstandard2.0\Albatross.CodeAnalysis.dll')
			$newElement.SetAttribute("IncludeRuntimeDependency", 'false')
			$node.AppendChild($newElement) | Out-Null
		}
		$doc.Save($csproj)
	}
}

function Run-Pack {
	param(
		[Parameter(Mandatory)]
		[string]$directory,
		[Parameter(Mandatory)]
		[string[]]$projects,
		[string[]]$codeGenProjects,
		[string]$nugetSource,
		[switch]
		[bool]$prod,
		[switch]
		[bool]$force,
		[switch]
		[bool]$noclean,
		[switch]
		[bool]$norevertchanges
	)
	process {
		if ((-not $force) -and (Test-GitDiff $directory)) {
			Write-Error "Please commit your changes before packing";
		}
		dotnet restore $directory --verbosity minimal
		
		$artifacts = "$directory\artifacts";
		if(-not $noclean) {
			Write-Information "Cleaning artifacts directory: $artifacts"
			Get-ChildItem $artifacts | foreach-object { Remove-Item $_.FullName; }
		}

		$versionFile = "$directory\Directory.Build.props";
		try{
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
			foreach ($project in $codeGenProjects) {
				$noHashVersion = $version.SubString(0, $version.IndexOf("+"));
				Update-CodeGenProjectReference -csproj "$directory\$project" -version $noHashVersion;
				Write-Information "Building $project";
				dotnet pack $directory\$project `
					--output $artifacts `
					--configuration release
			}

			$hasNugetPush = $false;
			if ($nugetSource) { 
				get-item $artifacts\*.nupkg | foreach-Object {
					nuget push $_.FullName -source $nugetSource -apiKey az
					if ($exitcode -ne 0) {
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
		}finally{
			if(-not $norevertchanges){
				Write-Information "Reverting changes";
				git checkout $versionFile;

				foreach ($project in $codeGenProjects) {
					git checkout $directory\$project;
				}
			}
		}
	}
}