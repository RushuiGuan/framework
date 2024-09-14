﻿using Microsoft.CodeAnalysis;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeAnalysis.Syntax;
using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Albatross.CodeGen.WebClient.CSharp {
	public class ConvertWebApiToCSharpCodeStack : IConvertObject<ControllerInfo, CodeStack> {
		const string ProxyService = "ProxyService";
		private readonly CSharpProxySettings settings;
		private readonly Compilation compilation;

		public ConvertWebApiToCSharpCodeStack(CSharpProxySettings settings, Compilation compilation) {
			this.settings = settings;
			this.compilation = compilation;
		}
		public CodeStack Convert(ControllerInfo from) {
			var codeStack = new CodeStack();
			using (codeStack.NewScope(new CompilationUnitBuilder())) {
				codeStack.With(new UsingDirectiveNode("System"))
					.With(new UsingDirectiveNode("System.Net.Http"))
					.With(new UsingDirectiveNode("System.Threading.Tasks"))
					.With(new UsingDirectiveNode("Microsoft.Extensions.Logging"))
					.With(new UsingDirectiveNode("Albatross.WebClient"))
					.With(new UsingDirectiveNode("System.Collections.Generic"))
					.With(new UsingDirectiveNode("Albatross.Serialization"));
				using (codeStack.NewScope(new NamespaceDeclarationBuilder(settings.Namespace))) {
					var proxyClassName = from.ControllerName + ProxyService;
					codeStack.FileName = $"{proxyClassName}.generated.cs";
					using (codeStack.NewScope(new ClassDeclarationBuilder(proxyClassName).Partial())) {
						codeStack.With(new BaseTypeNode("ClientBase"));
						using (codeStack.NewScope(new ConstructorDeclarationBuilder(proxyClassName))) {
							codeStack
								.With(new ParameterNode(false, new GenericIdentifierNode("ILogger", proxyClassName).Identifier, "logger"))
								.With(new ParameterNode("HttpClient", "client"));
							codeStack
								.Begin(new ArgumentListBuilder())
									.With(new IdentifierNode("logger"))
									.With(new IdentifierNode("client"))
								.End();
						}
						codeStack.Begin(new FieldDeclarationBuilder("string", "ControllerPath").Public().Const())
									.With(new LiteralNode(from.Route)).End();

						foreach (var method in from.Methods) {
							TypeSyntax returnType;
							if (method.ReturnType.SpecialType == SpecialType.System_Void) {
								returnType = new TypeNode("Task").Type;
							} else {
								returnType = new GenericIdentifierNode("Task", method.ReturnType.GetFullName()).Identifier;
							}
							using (codeStack.NewScope(new MethodDeclarationBuilder(returnType, method.Name))) {
								foreach (var param in method.Parameters) {
									codeStack.With(new ParameterNode(param.Type.GetFullName(), param.Name));
								}
								codeStack.Begin(new VariableBuilder("string", "path"))
									.Begin(new StringInterpolationBuilder())
										.With(new IdentifierNode("ControllerPath"))
										.With(new LiteralNode(method.Route))
									.End()
								.End();
								codeStack.Begin(new VariableBuilder("var", "queryString")).Complete(new NewObjectBuilder("NameValueCollection")).End();
								foreach (var param in method.Parameters.Where(x => x.WebType == ParameterType.FromQuery)) {
									using (codeStack.NewScope()) {
										if (param.IsArray) {
											using (codeStack.NewScope(new ForEachStatementBuilder(null, "item", param.Name))) {
												CreateAddQueryStringStatement(codeStack, param, "item");
											}
										} else {
											CreateAddQueryStringStatement(codeStack, param, null);
										}
									}
								}
								using (codeStack.NewScope(new UsingStatementBuilder())) {
									using (codeStack.NewScope(new VariableBuilder(null, "request"))) {
										codeStack.With(new ThisExpression());
										var fromBody = method.Parameters.FirstOrDefault(x => x.WebType == ParameterType.FromBody);
										if (fromBody == null) {
											codeStack.With(new IdentifierNode("CreateRequest"));
										} else if (fromBody.Type.SpecialType == SpecialType.System_String) {
											codeStack.With(new IdentifierNode("CreateStringRequest"));
										} else {
											codeStack.With(new GenericNameNode("CreateJsonRequest", fromBody.Type.GetFullName()));
										}
										using (codeStack.ToNewScope(new InvocationExpressionBuilder())) {
											using (codeStack.NewScope(new ArgumentListBuilder())) {
											}
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
		void CreateAddQueryStringStatement(CodeStack codeStack, ParameterInfo param, string? varaibleName) {
			varaibleName = varaibleName ?? param.Name;
			using (codeStack.With(new IdentifierNode("queryString")).ToNewScope(new InvocationExpressionBuilder("Add"))) {
				using (codeStack.NewScope(new ArgumentListBuilder())) {
					codeStack.With(new LiteralNode(param.QueryKey));
					if (param.Type.SpecialType == SpecialType.System_String) {
						codeStack.With(new IdentifierNode(varaibleName));
					} else {
						if (param.Type.SpecialType == SpecialType.System_DateTime || param.Type.GetFullName() == "System.DateTimeOffset") {
							codeStack.Begin()
								.With(new IdentifierNode(param.Name))
								.To(new InvocationExpressionBuilder("QueryString"))
							.End();
						} else {
							using (codeStack.NewScope(new StringInterpolationBuilder())) {
								switch (param.Type.GetFullName()) {
									case "System.DateOnly":
										codeStack.With(new StringInterpolationNode(varaibleName, "yyyy-MM-dd"));
										break;
									case "System.TimeOnly":
										codeStack.With(new StringInterpolationNode(varaibleName, "HH:mm:ss.fffffff"));
										break;
									default:
										codeStack.With(new IdentifierNode(varaibleName));
										break;
								}
							}
						}
					}
				}
			}
		}
		object IConvertObject<ControllerInfo>.Convert(ControllerInfo from) {
			return Convert(from);
		}
	}
}

