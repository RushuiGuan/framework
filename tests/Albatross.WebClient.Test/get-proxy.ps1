$location = "c:\temp\$([System.Guid]::NewGuid())";


dotnet publish $PSScriptRoot\..\Albatross.WebClient.TestApi\Albatross.WebClient.TestApi.csproj --output $location --self-contained --runtime win-x64;

$classes = (Invoke-ClassLoader -Namespaces Albatross.WebClient.TestApi `
        -Converter "Albatross.CodeGen.WebClient.ConvertApiControllerToCSharpClass, Albatross.CodeGen.WebClient" `
        -AssemblyFolders $location `
        -TargetAssembly $location\Albatross.WebClient.TestApi.dll)



$classes | ForEach-Object {
    $_.Namespace = "Albatross.WebClient.IntegrationTest";
    Invoke-CSharpClassGenerator -Output "$PSScriptRoot\$($_.Name).Generated.cs" -Class $_ -Force;
}

