using Albatross.CodeAnalysis.MSBuild;
using Albatross.CodeGen.Utility;
using Albatross.CodeGen.WebClient.CSharp;
using Albatross.CodeGen.WebClient.Models;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CommandLine {
	public class CSharpProxyCodeGenCommandHandler : ICommandHandler {
		private readonly ILogger<CSharpProxyCodeGenCommandHandler> logger;
		private readonly CodeGenCommandOptions options;
		private readonly Compilation compilation;
		private readonly ConvertApiControllerToControllerInfo convertToWebApi;
		private readonly ConvertWebApiToCSharpCodeStack converToCSharpCodeStack;

		public CSharpProxyCodeGenCommandHandler(IOptions<CodeGenCommandOptions> options, ILogger<CSharpProxyCodeGenCommandHandler> logger, Compilation compilation, ConvertApiControllerToControllerInfo convertToWebApi, ConvertWebApiToCSharpCodeStack converToCSharpFile) {
			this.options = options.Value;
			this.logger = logger;
			this.compilation = compilation;
			this.convertToWebApi = convertToWebApi;
			this.converToCSharpCodeStack = converToCSharpFile;
		}

		public int Invoke(InvocationContext context) {
			throw new System.NotSupportedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			var controllerClass = new List<INamedTypeSymbol>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var dtoClassWalker = new ApiControllerClassWalker(semanticModel);
				dtoClassWalker.Visit(syntaxTree.GetRoot());
				controllerClass.AddRange(dtoClassWalker.Result);
			}
			foreach (var controller in controllerClass) {
				if (string.IsNullOrEmpty(options.Controller) || string.Equals(controller.Name, options.Controller, System.StringComparison.InvariantCultureIgnoreCase)) {
					logger.LogInformation("Generating proxy for {controller}", controller.Name);
					var webApi = this.convertToWebApi.Convert(controller);
					var codeStack = this.converToCSharpCodeStack.Convert(webApi);

					var text = codeStack.BuildWithFormat();
					System.Console.WriteLine(text);
					if (options.OutputDirectory != null) {
						using (var writer = new System.IO.StreamWriter(System.IO.Path.Join(options.OutputDirectory.FullName, codeStack.FileName ?? "generated.cs"))) {
							writer.Write(text);
						}
					}
				}
			}
			return Task.FromResult(0);
		}
	}
}