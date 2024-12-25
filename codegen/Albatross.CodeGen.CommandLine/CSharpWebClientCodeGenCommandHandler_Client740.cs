using Albatross.CodeAnalysis.MSBuild;
using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.WebClient;
using Albatross.CodeGen.WebClient.CSharp;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeGen.WebClient.Settings;
using Albatross.CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CommandLine {
	public class CSharpWebClientCodeGenCommandHandler_Client740 : BaseHandler<CodeGenCommandOptions> {
		private readonly CreateHttpClientRegistrations createHttpClientRegistrations;
		private readonly Compilation compilation;
		private readonly CodeGenSettings settings;
		private readonly ILogger<CSharpWebClientCodeGenCommandHandler_Client740> logger;
		private readonly ConvertApiControllerToControllerModel convertToWebApi;
		private readonly ConvertWebApiToCSharpCodeStack_Client740 converToCSharpCodeStack;

		public CSharpWebClientCodeGenCommandHandler_Client740(IOptions<CodeGenCommandOptions> options,
			CreateHttpClientRegistrations createHttpClientRegistrations,
			Compilation compilation,
			CodeGenSettings settings,
			ILogger<CSharpWebClientCodeGenCommandHandler_Client740> logger,
			ConvertApiControllerToControllerModel convertToWebApi,
			ConvertWebApiToCSharpCodeStack_Client740 converToCSharpFile) : base(options) {
			this.createHttpClientRegistrations = createHttpClientRegistrations;
			this.compilation = compilation;
			this.settings = settings;
			this.logger = logger;
			this.convertToWebApi = convertToWebApi;
			this.converToCSharpCodeStack = converToCSharpFile;
		}

		public override Task<int> InvokeAsync(InvocationContext context) {
			var controllerClass = new List<INamedTypeSymbol>();
			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var dtoClassWalker = new ApiControllerClassWalker(semanticModel, settings.CreateCSharpControllerFilter());
				dtoClassWalker.Visit(syntaxTree.GetRoot());
				controllerClass.AddRange(dtoClassWalker.Result);
			}
			var models = new List<ControllerInfo>();
			foreach (var controller in controllerClass) {
				if (string.IsNullOrEmpty(options.AdhocFilter) || controller.GetFullName().Contains(options.AdhocFilter, System.StringComparison.InvariantCultureIgnoreCase)) {
					logger.LogInformation("Generating proxy for {controller}", controller.Name);
					var webApi = this.convertToWebApi.Convert(controller);
					webApi.ApplyMethodFilters(settings.CreateCSharpControllerMethodFilters());
					models.Add(webApi);
					var codeStack = this.converToCSharpCodeStack.Convert(webApi);

					var text = codeStack.BuildWithFormat();
					this.writer.WriteLine(text);
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