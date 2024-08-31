using Albatross.CodeGen.WebClient;
using Albatross.Logging;
using Albatross.CodeAnalysis.MSBuild;
using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript;
using Albatross.CodeGen.WebClient.TypeScript;
using Albatross.CommandLine;
using Albatross.Config;
using Albatross.Serialization;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace Albatross.CodeGen.CommandLine {
	public class MySetup : Setup{
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);

			services.AddScoped(provider => MSBuildWorkspace.Create());
			services.AddTypeScriptCodeGen().AddWebClientCodeGen();
			services.AddScoped<ICurrentProject>(provider => {
				var options = provider.GetRequiredService<IOptions<CodeGenCommandOptions>>();
				return new CurrentProject(options.Value.ProjectFile);
			});
			services.AddScoped<ICompilationFactory, MSBuildProjectCompilationFactory>();
			services.AddScoped<Compilation>(provider => provider.GetRequiredService<ICompilationFactory>().Create());
			services.AddShortenLoggerName(false, "Albatross");
			services.AddSingleton<TypeScriptWebClientSettings>(provider => {
				var options = provider.GetRequiredService<IOptions<CodeGenCommandOptions>>();
				if (string.IsNullOrEmpty(options.Value.SettingsFile)) {
					return new TypeScriptWebClientSettings();
				} else {
					using var stream = System.IO.File.OpenRead(options.Value.SettingsFile);
					var settings = JsonSerializer.Deserialize<TypeScriptWebClientSettings>(stream, DefaultJsonSettings.Value.Default) ?? throw new ArgumentException("Unable to deserialize typescript webclient settings");
					return settings;
				}
			});
			services.AddSingleton<ISourceLookup>(provider => {
				var settings = provider.GetRequiredService<TypeScriptWebClientSettings>();
				return new DefaultTypeScriptSourceLookup(settings.NameSpaceModuleMapping);
			});
		}
	}
}
