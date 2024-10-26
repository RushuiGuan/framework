using Albatross.CodeAnalysis.Syntax;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeGen.WebClient.Settings;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.CSharp {
	public class CreateHttpClientRegistrations {
		private readonly Compilation compilation;
		private readonly CodeGenSettings settings;

		public CreateHttpClientRegistrations(Compilation compilation, CodeGenSettings settings) {
			this.compilation = compilation;
			this.settings = settings;
		}

		public CodeStack Generate(IEnumerable<ControllerInfo> models) {
			var codeStack = new CodeStack() {
				FileName = "Extensions.generated.cs",
			};
			using (codeStack.NewScope(new CompilationUnitBuilder())) {
				codeStack.With(new UsingDirectiveNode("Microsoft.Extensions.DependencyInjection"));
				using (codeStack.NewScope(new NamespaceDeclarationBuilder(settings.CSharpWebClientSettings.Namespace))) {
					using (codeStack.NewScope(new ClassDeclarationBuilder("Extensions").Static().Partial())) {
						using (codeStack.NewScope(new MethodDeclarationBuilder("IHttpClientBuilder", "AddClients").Static())) {
							codeStack.With(new ParameterNode("IHttpClientBuilder", "builder").WithThis());
							using (codeStack.NewScope(new ReturnExpressionBuilder())) {
								codeStack.With(new IdentifierNode("builder"));
								foreach (var model in models) {
									codeStack.With(new GenericIdentifierNode("AddTypedClient", model.ControllerName + "ProxyService"));
									codeStack.To(new InvocationExpressionBuilder());
								}
							}
						}
					}
				}
			}
			return codeStack;
		}
	}
}