using Albatross.CodeAnalysis.MSBuild;
using Albatross.CodeGen.WebClient;
using Albatross.CodeGen.WebClient.CSharp;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeGen.WebClient.Settings;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CommandLine {
	public class CSharpWebClientCodeGenCommandHandler : ICommandHandler {
		private readonly ILogger<CSharpWebClientCodeGenCommandHandler> logger;
		private readonly CreateHttpClientRegistrations createHttpClientRegistrations;
		private readonly CodeGenCommandOptions options;
		private readonly Compilation compilation;
		private readonly CodeGenSettings settings;
		private readonly ConvertApiControllerToControllerInfo convertToWebApi;
		private readonly ConvertWebApiToCSharpCodeStack converToCSharpCodeStack;

		public CSharpWebClientCodeGenCommandHandler(IOptions<CodeGenCommandOptions> options, ILogger<CSharpWebClientCodeGenCommandHandler> logger,
			CreateHttpClientRegistrations createHttpClientRegistrations,
			Compilation compilation,
			CodeGenSettings settings,
			ConvertApiControllerToControllerInfo convertToWebApi,
			ConvertWebApiToCSharpCodeStack converToCSharpFile) {
			this.options = options.Value;
			this.logger = logger;
			this.createHttpClientRegistrations = createHttpClientRegistrations;
			this.compilation = compilation;
			this.settings = settings;
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
				var dtoClassWalker = new ApiControllerClassWalker(semanticModel, settings.CSharpControllerFilter);
				dtoClassWalker.Visit(syntaxTree.GetRoot());
				controllerClass.AddRange(dtoClassWalker.Result);
			}
			var models = new List<ControllerInfo>();
			foreach (var controller in controllerClass) {
				if (string.IsNullOrEmpty(options.Controller) || string.Equals(controller.Name, options.Controller, System.StringComparison.InvariantCultureIgnoreCase)) {
					logger.LogInformation("Generating proxy for {controller}", controller.Name);
					var webApi = this.convertToWebApi.Convert(controller);
					models.Add(webApi);
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
			BuildRegistrationMethod(models);
			return Task.FromResult(0);
		}
		void BuildRegistrationMethod(IEnumerable<ControllerInfo> models) {
			var registrationCodeStack = this.createHttpClientRegistrations.Generate(models);
			var text = registrationCodeStack.BuildWithFormat();
			System.Console.WriteLine(text);
			if (options.OutputDirectory != null) {
				using (var writer = new System.IO.StreamWriter(System.IO.Path.Join(options.OutputDirectory.FullName, registrationCodeStack.FileName ?? "Extensions.generated.cs"))) {
					writer.Write(text);
				}
			}
		}
	}
}