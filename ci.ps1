$propsFile = "$PSScriptRoot\Directory.Build.props";
$backup = "$PSScriptRoot\backup.props";

function Backup-DirectoryBuild() {
    $src = Get-Item $propsFile
    Copy-Item $src -Destination $backup;
}

function IsProd($branch) {
    if ($branch -eq "production" -or $branch -eq "prod") {
        return $true;
    }
    else {
        return $false;
    }
}

function Set-ReleaseVersion($branch, $build) {
    if (Test-Path $propsFile) {
        $xml = [xml](Get-Content $propsFile);
        $version = $xml.Project.PropertyGroup.Version
        if (-not (IsProd -branch $branch)) {
            $xml.Project.PropertyGroup.Version = $version + "-$branch.$build";
        }
        $xml.Save($propsFile);
        return $xml.Project.PropertyGroup.Version;
    }
    else {
        throw "Missing Directory.Build.props file";
    }
}

function Set-AngularReleaseVersion(
	[Parameter(ValueFromPipeline)]
	[string]$file, 
	[string]$version,
    [string]$branch) {
	if ([string]::IsNullOrEmpty($version)) { throw "Missing version";	}

    "Setting version $version for angular package $file";
	$json = (Get-Content $file | convertfrom-json);
	$json.version = $version;
	ConvertTo-Json $json | set-content $file;
}

function Get-NugetFeed($branch) {
    if (IsProd($branch)) {
        return "production";
    }
    else {
        return "staging";
    }
}

function Get-NpmFeed($branch) {
    if (IsProd($branch)) {
        return "angular-production";
    }
    else {
        return "angular-staging";
    }
}

function Get-OctopusReleaseNumber($branch, $build) {
    if (Test-Path $backup) {
        $xml = [xml](Get-Content $backup);
        if(IsProd($branch)){
            return $xml.Project.PropertyGroup.Version;
        }else{
            return $xml.Project.PropertyGroup.Version + "-$branch.$build";
        }
    }
    else {
        throw "Backup Directory.Build.props file has not been created";
    }
}

function Send-Octopus($file, $apiKey) {
    Add-Type -AssemblyName System.Net.Http
    $octopusURL = "http://octopus:8080/"
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
	if(-not $response){
        Write-Error "No response from Octopus. The OctopusDeploy service might be down";
    }
    $response.EnsureSuccessStatusCode() > $null;
    Write-Output "Successfully posted artifact $file to octopus";

    if ($fileStream) { $fileStream.Close() }
    if ($httpClient) { $httpClient.Dispose(); }
}

function Build-Angular() {
    Set-Location $PSScriptRoot\src\client
    npm ci;
    npm run deploy;
}