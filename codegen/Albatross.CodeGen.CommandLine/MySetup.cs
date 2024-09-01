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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace Albatross.CodeGen.CommandLine {
	public class MySetup : Setup{
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.RegisterCommands();

			services.AddScoped(provider => MSBuildWorkspace.Create());
			services.AddTypeScriptCodeGen().AddWebClientCodeGen();
			services.AddScoped<ICurrentProject>(provider => {
				var options = provider.GetRequiredService<IOptions<CodeGenCommandOptions>>().Value;
				if (options.ProjectFile.Exists) {
					return new CurrentProject(options.ProjectFile.FullName);
				} else {
					throw new InvalidOperationException($"File {options.ProjectFile.Name} doesn't exist");
				}
			});
			services.AddScoped<ICompilationFactory, MSBuildProjectCompilationFactory>();
			services.AddScoped(provider => provider.GetRequiredService<ICompilationFactory>().Create());
			services.AddShortenLoggerName(false, "Albatross");
			services.AddSingleton(provider => {
				var options = provider.GetRequiredService<IOptions<CodeGenCommandOptions>>().Value;
				if (options.SettingsFile == null) {
					return new TypeScriptWebClientSettings();
				} else if (options.SettingsFile.Exists) {
					using var stream = options.SettingsFile.OpenRead();
					var settings = JsonSerializer.Deserialize<TypeScriptWebClientSettings>(stream, DefaultJsonSettings.Value.Default) ?? throw new ArgumentException("Unable to deserialize typescript webclient settings");
					return settings;
				} else {
					throw new InvalidOperationException($"File {options.SettingsFile.Name} doesn't exist");
				}
			});
			services.AddSingleton<ISourceLookup>(provider => {
				var settings = provider.GetRequiredService<TypeScriptWebClientSettings>();
				return new DefaultTypeScriptSourceLookup(settings.NameSpaceModuleMapping);
			});
		}
	}
}
