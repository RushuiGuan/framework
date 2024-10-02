Set-StrictMode -Version Latest

Get-ChildItem readme.md -Recurse | ForEach-Object {
	if($_.FullName.EndsWith("readme.md")) {
		$newName = $_.FullName -replace 'readme.md', 'README1.md'
		Rename-Item -Path $_.FullName -NewName $newName
	}
}

git add .
git commit -m "Rename readme.md to README1.md"


Get-ChildItem README1.md -Recurse | ForEach-Object {
	$newName = $_.FullName -replace 'README1.md', 'README.md'
	Rename-Item -Path $_.FullName -NewName $newName
}

git add .
git commit -m "Rename README1.md back to README.md"