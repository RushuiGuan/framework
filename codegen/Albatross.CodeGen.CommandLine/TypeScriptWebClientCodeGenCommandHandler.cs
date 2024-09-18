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
		private readonly ConvertApiControllerToControllerInfo convertToWebApi;
		private readonly ConvertWebApiToTypeScriptFile converToTypeScriptFile;

		public TypeScriptWebClientCodeGenCommandHandler(IOptions<CodeGenCommandOptions> options, 
			ILogger<TypeScriptWebClientCodeGenCommandHandler> logger, 
			Compilation compilation, 
			CodeGenSettings settings,
			ConvertApiControllerToControllerInfo convertToWebApi, 
			ConvertWebApiToTypeScriptFile converToTypeScriptFile) {
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
			var controllerClass = new List<INamedTypeSymbol>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var dtoClassWalker = new ApiControllerClassWalker(semanticModel, settings.TypeScriptControllerFilter);
				dtoClassWalker.Visit(syntaxTree.GetRoot());
				controllerClass.AddRange(dtoClassWalker.Result);
			}
			foreach (var controller in controllerClass) {
				logger.LogInformation("Generating proxy for {controller}", controller.Name);
				var webApi = this.convertToWebApi.Convert(controller);
				var file = this.converToTypeScriptFile.Convert(webApi);
				file.Generate(System.Console.Out);
				if (options.OutputDirectory != null) {
					using (var writer = new System.IO.StreamWriter(System.IO.Path.Join(options.OutputDirectory.FullName, file.FileName))) {
						file.Generate(writer);
					}
				}
			}
			return Task.FromResult(0);
		}
	}
}