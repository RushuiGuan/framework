function ConvertRelativeUrl {
	param(
		[string]$rootUrl,
		[System.IO.FileInfo]$mdFile
	)
	
	$text = Get-Content .$mdFile.FullName -Raw

	$regex = "\[(.*?)\]\((.*?)\)"
    
    $matches = [regex]::Matches($text, $regex)
    
    foreach ($match in $matches) {
        $linkText = $match.Groups[1].Value
        $url = $match.Groups[2].Value
        Write-Output "Link Text: $linkText, URL: $url"
    }


}