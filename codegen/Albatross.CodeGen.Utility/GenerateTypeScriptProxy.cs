using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Utility {
	[Verb("typescript-proxy")]
	public class GenerateTypeScriptProxyOption : BaseOption {
		[Option('p', "project-file", Required = true)]
		public string ProjectFile { get; set; } = string.Empty;
	}
	public class GenerateTypeScriptProxy : UtilityBase<GenerateTypeScriptProxyOption> {
		public GenerateTypeScriptProxy(GenerateTypeScriptProxyOption option) : base(option) { }

		public async Task<int> RunUtility(ILogger<GenerateTypeScriptProxy> logger) {
			var workspace = MSBuildWorkspace.Create();
			Project project = await workspace.OpenProjectAsync(Options.ProjectFile);
			var compilation = await project.GetCompilationAsync();
			var dtoClasses = new List<INamedTypeSymbol>();
			var enums = new List<INamedTypeSymbol>();
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
