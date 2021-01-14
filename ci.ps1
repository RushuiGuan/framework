$file = "$PSScriptRoot\Directory.Build.props";
$backup = "$PSScriptRoot\backup.props";

function Backup-DirectoryBuild() {
    $src = Get-Item $file
    Copy-Item $src -Destination $backup;
}

function Set-PreRelease($prerelease, $build) {
    if (Test-Path $file) {
        $xml = [xml](Get-Content $file);
        $version = $xml.Project.PropertyGroup.Version
        $xml.Project.PropertyGroup.Version = $version + "-$prerelease.$build";
        $xml.Save($file);
        return $xml.Project.PropertyGroup.Version;
    }
    else {
        throw "Missing Directory.Build.props file";
    }
}

# octopus cannot handle prerelease.  So we use the format major.minor.patch.build instead.
# we will get rid of prerelease and add the build number
function Get-OctopusReleaseNumber($build) {
    if (Test-Path $backup) {
        $xml = [xml](Get-Content $backup);
        return $xml.Project.PropertyGroup.Version + ".$build";
    }
    else {
        throw "Backup Directory.Build.props file has not been created";
    }
}

function Send-Octopus($file, $apiKey) {
    Add-Type -AssemblyName System.Net.Http
    $octopusURL = "http://srv-build1:8888/Octopus/"
    # Create http client handler
    $httpClient = New-Object System.Net.Http.HttpClient
    $httpClient.DefaultRequestHeaders.Add("X-Octopus-ApiKey", $apiKey)

    # Open file stream
    $fileStream = New-Object System.IO.FileStream($file, [System.IO.FileMode]::Open)

    # Create dispositon object
    $contentDispositionHeaderValue = New-Object System.Net.Http.Headers.ContentDispositionHeaderValue "form-data"
    $contentDispositionHeaderValue.Name = "fileData"
    $contentDispositionHeaderValue.FileName = [System.IO.Path]::GetFileName($file)

    # Creat steam content
    $streamContent = New-Object System.Net.Http.StreamContent $fileStream
    $streamContent.Headers.ContentDisposition = $contentDispositionHeaderValue
    $contentType = "multipart/form-data"
    $streamContent.Headers.ContentType = New-Object System.Net.Http.Headers.MediaTypeHeaderValue $contentType

    $content = New-Object System.Net.Http.MultipartFormDataContent
    $content.Add($streamContent)

    # Upload package
    $response = $httpClient.PostAsync("$octopusURL/api/packages/raw?replace=false", $content).Result
    $response.EnsureSuccessStatusCode() > $null;
    Write-Output "Successfully posted artifact $file to octopus";

    if ($fileStream) { $fileStream.Close() }
    if ($httpClient) { $httpClient.Dispose(); }
}