param(
	[Parameter(Position=0)]
	[string]$project,
	[switch]
	[bool]$prod,
	[switch]
	[bool]$nopush,
	[switch]
	[bool]$force,
	[switch]
	[bool]$noclean,
	[switch]
	[bool]$norevertchanges
)
Set-StrictMode -Version Latest;
$InformationPreference = "Continue";
$ErrorActionPreference = "Stop";
. $PSScriptRoot\scripts\pack.ps1;

$projects = @(
	"authentication\Albatross.Authentication",
	"authentication\Albatross.Authentication.AspNetCore",
	"authentication\Albatross.Authentication.Windows",

	"caching\Albatross.Caching",
	"caching\Albatross.Caching.Controllers",
	"caching\Albatross.Caching.MemCache",
	"caching\Albatross.Caching.Redis",
	"caching\Albatross.EFCore.AutoCacheEviction",

	"codeanalysis\Albatross.CodeAnalysis",
	"codeanalysis\Albatross.CodeAnalysis.MSBuild",

	"codegen\Albatross.CodeGen",
	"codegen\Albatross.CodeGen.CSharp",
	"codegen\Albatross.CodeGen.TypeScript",
	"codegen\Albatross.CodeGen.WebClient",

	"collections\Albatross.Collections",
	
	"commandline\Albatross.CommandLine",
	"commandline\Albatross.Hosting.Utility",
	
	"config\Albatross.Config",
	"dates\Albatross.Dates",
	"datelevel\Albatross.DateLevel",
	
	"efcore\Albatross.EFCore",
	"efcore\Albatross.EFCore.Audit",
	"efcore\Albatross.EFCore.ChangeReporting",
	"efcore\Albatross.EFCore.PostgreSQL",
	"efcore\Albatross.EFCore.SqlServer",
	
	"excel\Albatross.Excel",
	"excel\Albatross.Hosting.Excel",
	
	"hosting\Albatross.Hosting",
	
	"testing\Albatross.Testing",
	"testing\Albatross.Testing.EFCore",
	"testing\Albatross.Reqnroll",
	"testing\Albatross.ReqnrollPlugin",

	"io\Albatross.IO",
	
	"logging\Albatross.Logging",
	"math\Albatross.Math",
	"messaging\Albatross.Messaging",
	"reflection\Albatross.Reflection",
	"serialization\Albatross.Serialization",
	"text\Albatross.Text",
	"threading\Albatross.Threading",
	"webclient\Albatross.WebClient"
);
$codeGenProjects = @(
	"commandline\Albatross.CommandLine.CodeGen\Albatross.CommandLine.CodeGen.csproj",
	"messaging\Albatross.Messaging.CodeGen\Albatross.Messaging.CodeGen.csproj",
	"efcore\Albatross.EFCore.CodeGen\Albatross.EFCore.CodeGen.csproj"
);

$utilityProjects  = @(
	"codegen\Albatross.CodeGen.CommandLine\Albatross.CodeGen.CommandLine.csproj",
	"devtools\Albatross.DevTools\Albatross.DevTools.csproj"
);

if(-not [string]::IsNullOrEmpty($project)){
	$projects = $projects | Where-Object { $_ -like "$project*" }
	$codeGenProjects = $codeGenProjects | Where-Object { $_ -like "$project*" }
	$utilityProjects = $utilityProjects | Where-Object { $_ -like "$project*" }
}

$nugetSource = $env:DefaultNugetSource;
if($nopush){
	$nugetSource = $null;
}

Run-Pack -projects $projects `
	-codeGenProjects $codeGenProjects `
	-utilityProjects $utilityProjects `
	-directory $PSScriptRoot `
	-nugetSource $nugetSource `
	-prod:$prod `
	-force:$force `
	-noclean:$noclean `
	-norevertchanges:$norevertchanges
