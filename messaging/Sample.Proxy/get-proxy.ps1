get-item $PSScriptRoot\*.Generated.cs | remove-item;
codegen csharp-proxy -p $PSScriptRoot\..\Sample.WebApi\Sample.WebApi.csproj -s $PSScriptRoot\codegen-settings.json -o $PSScriptRoot\
