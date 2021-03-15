dotnet publish $PSScriptRoot\..\Albatross.Hosting\Albatross.Hosting.csproj --output c:\temp\Albatross.Hosting --self-contained --runtime win-x64;


$classes = (Invoke-ClassLoader -Namespaces Albatross.Hosting `
        -Converter "Albatross.CodeGen.WebClient.ConvertApiControllerToCSharpClass, Albatross.CodeGen.WebClient" `
        -AssemblyFolders C:\temp\Albatross.Hosting `
        -TargetAssembly C:\temp\Albatross.Hosting\Albatross.Hosting.dll)



$classes | ForEach-Object {
    if($_.Name -eq "CachingProxyService"){
        $_.Namespace = "Albatross.Caching";
        Invoke-CSharpClassGenerator -Output "$PSScriptRoot\$($_.Name).Generated.cs" -Class $_ -Force;
    }
}

