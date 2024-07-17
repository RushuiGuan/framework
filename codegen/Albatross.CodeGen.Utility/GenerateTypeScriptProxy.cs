using Albatross.CodeGen.WebClient.TypeScript;
using CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Utility {
	[Verb("typescript-proxy")]
	public class GenerateTypeScriptProxyOption : MyUtilityOption { }
	public class GenerateTypeScriptProxy : MyUtilityBase<GenerateTypeScriptProxyOption> {
		public GenerateTypeScriptProxy(GenerateTypeScriptProxyOption option) : base(option) { }

		public Task<int> RunUtility(Compilation compilation, ConvertApiControllerToTypeScriptFile converter) {
			var controllerClass = new List<INamedTypeSymbol>();

			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var dtoClassWalker = new ApiControllerClassWalker(semanticModel);
				dtoClassWalker.Visit(syntaxTree.GetRoot());
				controllerClass.AddRange(dtoClassWalker.Result);
			}
			foreach (var controller in controllerClass) {
				logger.LogInformation("Generating proxy for {controller}", controller.Name);
				var file = converter.Convert(controller);
				file.Generate(System.Console.Out);
				if (!string.IsNullOrEmpty(Options.OutputDirectory)) {
					using (var writer = new System.IO.StreamWriter(System.IO.Path.Join(Options.OutputDirectory, file.FileName))) {
						file.Generate(writer);
					}
				}
			}
			return Task.FromResult(0);
		}
	}
}
