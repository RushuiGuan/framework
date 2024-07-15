using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.Config;
using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Albatross.CodeGen.TypeScript;
using Albatross.CodeAnalysis.MSBuild;

namespace Albatross.CodeGen.Utility {
	[Verb("typescript-dto")]
	public class GenerateTypeScriptDtoOption : BaseOption {
		[Option('p', "project-file", Required = true)]
		public string ProjectFile { get; set; } = string.Empty;

		[Option('o', "output-directory")]
		public string? OutputDirectory { get; set; }
	}
	public class GenerateTypeScriptDto : UtilityBase<GenerateTypeScriptDtoOption> {
		public GenerateTypeScriptDto(GenerateTypeScriptDtoOption option) : base(option) { }

		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.AddScoped(provider => MSBuildWorkspace.Create());
			services.AddTypeScriptCodeGen();
			services.AddScoped<ICurrentProject>(provider => new CurrentProject(Options.ProjectFile));
			services.AddScoped<ICompilationFactory, MSBuildProjectCompilationFactory>();
			services.AddScoped<Compilation>(provider => provider.GetRequiredService<ICompilationFactory>().Create());
		}
		public Task<int> RunUtility(ILogger<GenerateTypeScriptDto> logger,
			Compilation compilation,
			IConvertObject<INamedTypeSymbol, InterfaceDeclaration> interfaceConverter,
			IConvertObject<INamedTypeSymbol, EnumDeclaration> enumConverter) {
			var dtoClasses = new List<INamedTypeSymbol>();
			var enums = new List<INamedTypeSymbol>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var dtoClassWalker = new DtoClassWalker(semanticModel);
				dtoClassWalker.Visit(syntaxTree.GetRoot());
				dtoClasses.AddRange(dtoClassWalker.Result);

				var enumWalker = new EnumTypeWalker(semanticModel);
				enumWalker.Visit(syntaxTree.GetRoot());
				enums.AddRange(enumWalker.Result);
			}
			var enumFile = new TypeScriptFileDeclaration("enum") {
				EnumDeclarations = enums.Select(x => enumConverter.Convert(x)).ToList(),
			};
			enumFile.Generate(System.Console.Out);
			var dtoFile = new TypeScriptFileDeclaration("dto") {
				InterfaceDeclarations = dtoClasses.Select(x => interfaceConverter.Convert(x)).ToList(),
			};
			dtoFile.Generate(System.Console.Out);
			if (!string.IsNullOrEmpty(Options.OutputDirectory)) {
				using (var writer = new StreamWriter(Path.Join(Options.OutputDirectory, enumFile.FileName))) {
					enumFile.Generate(writer);
				}
				using(var writer = new StreamWriter(Path.Join(Options.OutputDirectory, dtoFile.FileName))) {
					dtoFile.Generate(writer);
				}
			}
			return Task.FromResult(0);
		}
	}
}
