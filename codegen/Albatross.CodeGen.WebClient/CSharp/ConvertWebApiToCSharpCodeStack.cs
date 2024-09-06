using Microsoft.CodeAnalysis;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeAnalysis.Syntax;
using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeGen.WebClient.CSharp {
	public class ConvertWebApiToCSharpCodeStack: IConvertObject<WebApi, CodeStack> {
		const string ProxyService = "ProxyService";
		private readonly CSharpProxySettings settings;
		private readonly Compilation compilation;

		public ConvertWebApiToCSharpCodeStack(CSharpProxySettings settings, Compilation compilation) {
			this.settings = settings;
			this.compilation = compilation;
		}
		public CodeStack Convert(WebApi from) {
			var codeStack = new CodeStack();
			using (codeStack.Begin(new CompilationUnitBuilder()).NewScope()) {
				codeStack.With(new UsingDirectiveNode("System"))
					.With(new UsingDirectiveNode("System.Net.Http"))
					.With(new UsingDirectiveNode("System.Threading.Tasks"))
					.With(new UsingDirectiveNode("Microsoft.Extensions.Logging"))
					.With(new UsingDirectiveNode("Albatross.WebClient"))
					.With(new UsingDirectiveNode("System.Collections.Generic"))
					.With(new UsingDirectiveNode("Albatross.Serialization"));
				using (codeStack.Begin(new NamespaceDeclarationBuilder(settings.Namespace)).NewScope()) {
					var proxyClassName = from.ControllerName + ProxyService;
					codeStack.FileName = $"{proxyClassName}.generated.cs";
					using(codeStack.Begin(new ClassDeclarationBuilder(proxyClassName).Partial()).NewScope()) {
						codeStack.With(new BaseTypeNode("ClientBase"));
						using (codeStack.Begin(new ConstructorDeclarationBuilder(proxyClassName)).NewScope()) {
							codeStack
								.With(new ParameterNode(false, new GenericIdentifierNode("ILogger", proxyClassName).Identifier, "logger"))
								.With(new ParameterNode("HttpClient", "client"));
							codeStack
								.Begin(new ArgumentListBuilder())
									.With(new IdentifierNode("logger"))
									.With(new IdentifierNode("client"))
								.End();
						}
						foreach (var method in from.Methods) {
							TypeSyntax returnType;
							if (method.ReturnType.SpecialType == SpecialType.System_Void) {
								returnType = new GenericIdentifierNode("Task").Identifier;
							} else {
								returnType = new GenericIdentifierNode("Task", method.ReturnType.GetFullName()).Identifier;
							}
							using (codeStack.Begin(new MethodDeclarationBuilder(returnType, method.Name)).NewScope()) {
								foreach(var param in method.Parameters) {
									codeStack.With(new ParameterNode(param.Type.GetFullName(), param.Name));
								}
							}
						}
					}
				}
			}
			return codeStack;
		}
		object IConvertObject<WebApi>.Convert(WebApi from) {
			return Convert(from);
		}
	}
}
