param(
	[Parameter(Position=0)]
	[string]$project,
	[switch]
	[bool]$prod,
	[switch]
	[bool]$nopush,
	[switch]
	[bool]$force
)
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
	"collections\Albatross.Collections",
	"commandline\Albatross.CommandLine",
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
	"hosting\Albatross.Hosting.Test",
	"hosting\Albatross.Hosting.Utility",
	"specflow\Albatross.SpecFlow",
	"specflow\Albatross.SpecFlowPlugin",
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
	"commandline\Albatross.CommandLine.CodeGen",
	"messaging\Albatross.Messaging.CodeGen"
);

if(-not [string]::IsNullOrEmpty($project)){
	$projects = $projects | Where-Object { $_ -like "$project*" }
}

$nugetSource = $env:DefaultNugetSource;
if($nopush){
	$nugetSource = $null;
}

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-nugetSource $nugetSource `
	-prod:$prod `
	-force:$force
