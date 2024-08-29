using Albatross.Config;
using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Albatross.CodeGen.TypeScript;
using Albatross.CodeAnalysis.MSBuild;
using Albatross.CodeGen.Syntax;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Albatross.CodeGen.WebClient.TypeScript;
using Albatross.CodeGen.WebClient;
using Albatross.Serialization;
using Albatross.Logging;
using Albatross.CodeAnalysis.Symbols;

namespace Albatross.CodeGen.Utility {
	public class MyUtilityOption : BaseOption {
		[Option('p', "project-file", Required = true)]
		public string ProjectFile { get; set; } = string.Empty;

		[Option('s', "settings", Required = false)]
		public string? SettingsFile { get; set; }

		[Option('o', "output-directory")]
		public string? OutputDirectory { get; set; }
	}
	public class MyUtilityBase<T> : UtilityBase<T> where T:MyUtilityOption {
		public MyUtilityBase(T option) : base(option) { }

		public override async Task Init(IConfiguration configuration, IServiceProvider provider) {
			await base.Init(configuration, provider);
			var compilation = provider.GetRequiredService<Compilation>();
			var errors = compilation.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).ToArray();
			if (errors.Any()) {
				logger.LogError("Project {name} has error:\n\t{error}", Options.ProjectFile, string.Join("\n\t", errors.Select(x => x.GetMessage())));
			}
		}
		
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.AddScoped(provider => MSBuildWorkspace.Create());
			services.AddTypeScriptCodeGen().AddWebClientCodeGen();
			services.AddScoped<ICurrentProject>(provider => new CurrentProject(Options.ProjectFile));
			services.AddScoped<ICompilationFactory, MSBuildProjectCompilationFactory>();
			services.AddScoped<Compilation>(provider => provider.GetRequiredService<ICompilationFactory>().Create());
			services.AddShortenLoggerName(false, "Albatross");
			if (string.IsNullOrEmpty(Options.SettingsFile)) {
				services.AddSingleton<ISourceLookup>(new DefaultTypeScriptSourceLookup(new Dictionary<string, string>()));
				services.AddSingleton(new TypeScriptWebClientSettings());
			} else {
				using var stream = System.IO.File.OpenRead(Options.SettingsFile);
				var settings = JsonSerializer.Deserialize<TypeScriptWebClientSettings>(stream, DefaultJsonSettings.Value.Default) ?? throw new ArgumentException("Unable to deserialize typescript webclient settings");
				services.AddSingleton<ISourceLookup>(new DefaultTypeScriptSourceLookup(settings.NameSpaceModuleMapping));
				services.AddSingleton(settings);
			}
		}
	}
}
