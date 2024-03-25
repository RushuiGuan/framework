param(
	[Parameter(Position=0)]
	[string]$project,
	[switch]
	[bool]$prod,
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
	"codegen\Albatross.CodeGen",
	"codegen\Albatross.CodeGen.CSharp",
	"codegen\Albatross.CodeGen.TypeScript",
	"codegen\Albatross.CodeGen.WebClient",
	"collections\Albatross.Collections",
	"config\Albatross.Config",
	"dates\Albatross.Dates",
	"efcore\Albatross.DateLevel",
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

if(-not [string]::IsNullOrEmpty($project)){
	$projects = $projects | Where-Object { $_ -like "*$project" }
}

Run-Pack -projects $projects `
	-directory $PSScriptRoot `
	-nugetSource $env:DefaultNugetSource `
	-prod:$prod `
	-force:$force
