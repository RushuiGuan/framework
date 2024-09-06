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
	public class CSharpProxyCodeGenHandler : ICommandHandler {
		private readonly ILogger<CSharpProxyCodeGenHandler> logger;
		private readonly CodeGenCommandOptions options;
		private readonly Compilation compilation;
		private readonly ConvertApiControllerToWebApi convertToWebApi;
		private readonly ConvertWebApiToCSharpCodeStack converToCSharpCodeStack;

		public CSharpProxyCodeGenHandler(IOptions<CodeGenCommandOptions> options, ILogger<CSharpProxyCodeGenHandler> logger, Compilation compilation, ConvertApiControllerToWebApi convertToWebApi, ConvertWebApiToCSharpCodeStack converToCSharpFile) {
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
				logger.LogInformation("Generating proxy for {controller}", controller.Name);
				var webApi = this.convertToWebApi.Convert(controller);
				var codeStack = this.converToCSharpCodeStack.Convert(webApi);
				var text = codeStack.Build();
				System.Console.WriteLine(text);
				if (options.OutputDirectory != null) {
					using (var writer = new System.IO.StreamWriter(System.IO.Path.Join(options.OutputDirectory.FullName, codeStack.FileName ?? "generated.cs"))) {
						writer.Write(text);
					}
				}
			}
			return Task.FromResult(0);
		}
	}
}