$InformationPreference = "Continue";
$install = $env:InstallDirectory;



$solutionRoot = get-paren



# dotnet restore $PSScriptRoot
# dotnet publish $PSScriptRoot\$project\$project.csproj -o $install\$project -c release -r win-x64