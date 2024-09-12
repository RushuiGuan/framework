$project = Get-Item $PSScriptRoot\..\commandline\albatross.commandline.codegen\albatross.commandline.codegen.csproj;

[xml]$doc = Get-Content $project.FullName;

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
$newElement.SetAttribute("Version", '1.0.1')
$newElement.SetAttribute("GeneratePathProperty", 'true')
$newElement.SetAttribute("PrivateAssets", 'all')
$node.AppendChild($newElement) | Out-Null

$node = Select-Xml -Xml $doc -XPath '//Project/Target/ItemGroup/TargetPathWithTargetPlatformMoniker[@Include="$(PKGAlbatross_CodeAnalysis)\lib\netstandard2.0\Albatross.CodeAnalysis.dll"]' | Select-Object -ExpandProperty Node
if (-not $node) {
	$node = Select-Xml -Xml $doc -XPath '//Project/Target/ItemGroup' | Select-Object -ExpandProperty Node
	$newElement = $doc.CreateElement("TargetPathWithTargetPlatformMoniker")
	$newElement.SetAttribute("Include", '$(PKGAlbatross_CodeAnalysis)\lib\netstandard2.0\Albatross.CodeAnalysis.dll')
	$newElement.SetAttribute("IncludeRuntimeDependency", 'false')
	$node.AppendChild($newElement) | Out-Null
}

$doc.Save($project)