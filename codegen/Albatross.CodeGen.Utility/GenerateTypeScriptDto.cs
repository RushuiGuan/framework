using Albatross.CodeAnalysis;
using Albatross.Config;
using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Utility {
	[Verb("typescript-dto")]
	public class GenerateTypeScriptDtoOption : BaseOption {
		[Option('p', "project-file", Required = true)]
		public string ProjectFile { get; set; } = string.Empty;

		[Option('o', "output-file")]
		public string? OutputFile { get; set; }
	}
	public class GenerateTypeScriptDto : UtilityBase<GenerateTypeScriptDtoOption> {
		public GenerateTypeScriptDto(GenerateTypeScriptDtoOption option) : base(option) { }

		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.AddScoped(provider => MSBuildWorkspace.Create());
			services.AddScoped<ICurrentProject>(provider => new CurrentProject(Options.ProjectFile));
			services.AddScoped<ICompilationFactory, ProjectCompilationFactory>();
		}
		public async Task<int> RunUtility(ILogger<GenerateTypeScriptDto> logger, ICompilationFactory compilationFactory) {
			var dtoClasses = new List<INamedTypeSymbol>();
			var enums = new List<INamedTypeSymbol>();
			var compilation = await compilationFactory.Get();
			if (compilation != null) {
				foreach (var syntaxTree in compilation.SyntaxTrees) {
					var semanticModel = compilation.GetSemanticModel(syntaxTree);
					var dtoClassWalker = new DtoClassWalker(semanticModel);
					dtoClassWalker.Visit(syntaxTree.GetRoot());
					dtoClasses.AddRange(dtoClassWalker.Result);

					var enumWalker = new EnumTypeWalker(semanticModel);
					enumWalker.Visit(syntaxTree.GetRoot());
					enums.AddRange(enumWalker.Result);
				}
			}
			dtoClasses.ForEach(x=>Options.WriteOutput(x.Name));
			return 0;
		}
	}
}
