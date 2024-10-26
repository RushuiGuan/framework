using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.WebClient;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeGen.WebClient.Settings;
using Albatross.CodeGen.WebClient.TypeScript;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CommandLine {
	public class TypeScriptWebClientCodeGenCommandHandler : ICommandHandler {
		private readonly ILogger<TypeScriptWebClientCodeGenCommandHandler> logger;
		private readonly CodeGenCommandOptions options;
		private readonly Compilation compilation;
		private readonly CodeGenSettings settings;
		private readonly ConvertApiControllerToControllerModel convertToWebApi;
		private readonly ConvertControllerModelToTypeScriptFile converToTypeScriptFile;

		public TypeScriptWebClientCodeGenCommandHandler(IOptions<CodeGenCommandOptions> options,
			ILogger<TypeScriptWebClientCodeGenCommandHandler> logger,
			Compilation compilation,
			CodeGenSettings settings,
			ConvertApiControllerToControllerModel convertToWebApi,
			ConvertControllerModelToTypeScriptFile converToTypeScriptFile) {
			this.options = options.Value;
			this.logger = logger;
			this.compilation = compilation;
			this.settings = settings;
			this.convertToWebApi = convertToWebApi;
			this.converToTypeScriptFile = converToTypeScriptFile;
		}

		public int Invoke(InvocationContext context) {
			throw new System.NotSupportedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			var models = new List<INamedTypeSymbol>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var dtoClassWalker = new ApiControllerClassWalker(semanticModel, settings.CreateTypeScriptControllerFilter());
				dtoClassWalker.Visit(syntaxTree.GetRoot());
				models.AddRange(dtoClassWalker.Result);
			}
			var files = new List<TypeScriptFileDeclaration>();
			foreach (var model in models) {
				if (string.IsNullOrEmpty(options.AdhocFilter) || model.GetFullName().Contains(options.AdhocFilter)) {
					logger.LogInformation("Generating proxy for {controller}", model.Name);
					var webApi = this.convertToWebApi.Convert(model);
					webApi.ApplyMethodFilters(settings.CreateTypeScriptControllerMethodFilters());
					var file = this.converToTypeScriptFile.Convert(webApi);
					file.Generate(System.Console.Out);
					files.Add(file);
					logger.LogInformation("directory: {data}", options.OutputDirectory?.FullName);
					if (options.OutputDirectory != null) {
						using (var writer = new System.IO.StreamWriter(System.IO.Path.Join(options.OutputDirectory.FullName, file.FileName))) {
							file.Generate(writer);
						}
					}
				}
			}
			return Task.FromResult(0);
		}
	}
}