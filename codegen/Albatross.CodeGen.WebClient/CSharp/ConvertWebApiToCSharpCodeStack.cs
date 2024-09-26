﻿using Microsoft.CodeAnalysis;
using Albatross.CodeGen.WebClient.Models;
using Albatross.CodeAnalysis.Syntax;
using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Albatross.CodeGen.WebClient.Settings;

namespace Albatross.CodeGen.WebClient.CSharp {
	public class ConvertWebApiToCSharpCodeStack : IConvertObject<ControllerInfo, CodeStack> {
		const string ProxyService = "ProxyService";
		private readonly Compilation compilation;
		private readonly CodeGenSettings settings;

		public ConvertWebApiToCSharpCodeStack(Compilation compilation, CodeGenSettings settings) {
			this.compilation = compilation;
			this.settings = settings;
		}
		public CodeStack Convert(ControllerInfo from) {
			var codeStack = new CodeStack();
			using (codeStack.NewScope(new CompilationUnitBuilder())) {
				codeStack
					.With(new UsingDirectiveNode("Albatross.Dates"))
					.With(new UsingDirectiveNode("System.Net.Http"))
					.With(new UsingDirectiveNode("System.Threading.Tasks"))
					.With(new UsingDirectiveNode("Microsoft.Extensions.Logging"))
					.With(new UsingDirectiveNode("Albatross.WebClient"))
					.With(new UsingDirectiveNode("System.Collections.Specialized"));

				using (codeStack.NewScope(new NamespaceDeclarationBuilder(settings.CSharpWebClientSettings.Namespace))) {
					var proxyClassName = from.ControllerName + ProxyService;
					codeStack.FileName = $"{proxyClassName}.generated.cs";

					using (codeStack.NewScope(new ClassDeclarationBuilder(proxyClassName).Partial())) {
						codeStack.With(new BaseTypeNode("ClientBase"));
						using (codeStack.NewScope(new ConstructorDeclarationBuilder(proxyClassName))) {
							codeStack
								.With(new ParameterNode(new GenericIdentifierNode("ILogger", proxyClassName), "logger"))
								.With(new ParameterNode("HttpClient", "client"));
							codeStack
								.Begin(new ArgumentListBuilder())
									.With(new IdentifierNode("logger"))
									.With(new IdentifierNode("client"))
								.End();
						}
						codeStack.Begin(new FieldDeclarationBuilder("string", "ControllerPath").Public().Const())
								.With(new LiteralNode(from.Route))
							.End();

						foreach (var method in from.Methods) {
							TypeSyntax returnType;
							if (method.ReturnType.SpecialType == SpecialType.System_Void) {
								returnType = new TypeNode("Task").Type;
							} else {
								returnType = new GenericIdentifierNode("Task", method.ReturnType.AsTypeNode()).Type;
							}
							using (codeStack.NewScope(new MethodDeclarationBuilder(returnType, method.Name).Async())) {
								foreach (var param in method.Parameters) {
									codeStack.With(new ParameterNode(param.Type.AsTypeNode(), param.Name));
								}
								using (codeStack.NewScope(new VariableBuilder("string", "path"))) {
									using (codeStack.NewScope(new StringInterpolationBuilder())) {
										codeStack.With(new IdentifierNode("ControllerPath"));
										if (method.RouteSegments.Any()) {
											codeStack.With(new LiteralNode(@"/"));
										}
										foreach (var routeSegment in method.RouteSegments) {
											BuildRouteSegment(codeStack, method, routeSegment);
										}
									}
								}
								codeStack.Begin(new VariableBuilder("var", "queryString")).Complete(new NewObjectBuilder("NameValueCollection")).End();
								foreach (var param in method.Parameters.Where(x => x.WebType == ParameterType.FromQuery)) {
									using (codeStack.NewScope()) {
										if (param.Type.TryGetCollectionElementType(out var elementType)) {
											using (codeStack.NewScope(new ForEachStatementBuilder(null, "item", param.Name))) {
												CreateAddQueryStringStatement(codeStack, method.Settings, elementType!, param.QueryKey, "item");
											}
										} else {
											CreateAddQueryStringStatement(codeStack, method.Settings, param.Type, param.QueryKey, param.Name);
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
											codeStack.With(new GenericIdentifierNode("CreateJsonRequest", fromBody.Type.AsTypeNode()));
										}
										using (codeStack.ToNewScope(new InvocationExpressionBuilder())) {
											using (codeStack.NewScope(new ArgumentListBuilder())) {
												codeStack.With(new IdentifierNode("HttpMethod"))
													.With(new IdentifierNode(method.HttpMethod))
													.To(new MemberAccessBuilder());
												codeStack.With(new IdentifierNode("path"));
												codeStack.With(new IdentifierNode("queryString"));
												if (fromBody != null) {
													codeStack.With(new IdentifierNode(fromBody.Name));
												}
											}
										}
									}
									if (method.ReturnType.SpecialType == SpecialType.System_Void) {
										using (codeStack.NewScope()) {
											codeStack.With(new ThisExpression()).ToNewBegin(new InvocationExpressionBuilder("GetRawResponse").Await())
														.Begin(new ArgumentListBuilder())
															.With(new IdentifierNode("request"))
														.End()
													.End();
										}
									} else {
										using (codeStack.NewScope(new ReturnExpressionBuilder())) {
											if (method.ReturnType.SpecialType == SpecialType.System_String) {
												codeStack.With(new ThisExpression()).ToNewBegin(new InvocationExpressionBuilder("GetRawResponse").Await())
													.Begin(new ArgumentListBuilder())
														.With(new IdentifierNode("request"))
													.End()
												.End();
											} else {
												string functionName;
												if (method.ReturnType.IsNullable()) {
													functionName = "GetJsonResponse";
												} else if (method.ReturnType.IsValueType) {
													functionName = "GetRequiredJsonResponseForValueType";
												} else {
													functionName = "GetRequiredJsonResponse";
												}
												codeStack.With(new ThisExpression())
													.With(new GenericIdentifierNode(functionName, method.ReturnType.AsTypeNode()))
													.ToNewBegin(new InvocationExpressionBuilder().Await())
													.Begin(new ArgumentListBuilder())
														.With(new IdentifierNode("request"))
													.End()
												.End();
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
		CodeStack BuildRouteSegment(CodeStack cs, MethodInfo method, IRouteSegment routeSegment) {
			using (cs.NewScope()) {
				if (routeSegment is RouteParameterSegment routeParam) {
					var type = routeParam.ParameterInfo?.Type.GetFullName();
					if (type == "System.DateTime" && method.Settings.UseDateTimeAsDateOnly == true) {
						cs.With(new IdentifierNode(routeParam.Text)).To(new InvocationExpressionBuilder("ISO8601StringDateOnly"));
					} else if (type == "System.DateTime" || type == "System.DateTimeOffset" || type == "System.DateOnly" || type == "System.TimeOnly") {
						cs.With(new IdentifierNode(routeParam.Text)).To(new InvocationExpressionBuilder("ISO8601String"));
					} else {
						cs.With(new IdentifierNode(routeParam.Text));
					}
				} else {
					cs.With(new LiteralNode(routeSegment.Text));
				}
			}
			return cs;
		}
		CodeStack CreateAddQueryStringStatement(CodeStack codeStack, WebClientMethodSettings settings, ITypeSymbol type, string queryKey, string variableName) {
			ITypeSymbol finalType = type;
			if (type.IsNullable()) {
				codeStack.Begin(new IfStatementBuilder());
				codeStack.With(new NotEqualStatementNode(new IdentifierNode(variableName), new NullExpressionNode()));

				if (type.TryGetNullableValueType(out var valueType)) {
					finalType = valueType!;
				}
			}

			using (codeStack.NewScope()) {
				using (codeStack.With(new IdentifierNode("queryString")).ToNewScope(new InvocationExpressionBuilder("Add"))) {
					using (codeStack.NewScope(new ArgumentListBuilder())) {
						codeStack.With(new LiteralNode(queryKey));
						if (finalType.SpecialType == SpecialType.System_String) {
							codeStack.With(new IdentifierNode(variableName));
						} else {
							if (finalType.SpecialType == SpecialType.System_DateTime && settings.UseDateTimeAsDateOnly == true) {
								using (codeStack.NewScope()) {
									codeStack.With(new IdentifierNode(variableName));
									if (type.IsNullableValueType()) {
										codeStack.With(new IdentifierNode("Value"));
									}
									codeStack.To(new InvocationExpressionBuilder("ISO8601StringDateOnly"));
								}
							} else if (finalType.SpecialType == SpecialType.System_DateTime
								|| finalType.GetFullName() == "System.DateTimeOffset"
								|| finalType.GetFullName() == "System.DateOnly"
								|| finalType.GetFullName() == "System.TimeOnly") {
								using (codeStack.NewScope()) {
									codeStack.With(new IdentifierNode(variableName));
									if (type.IsNullableValueType()) {
										codeStack.With(new IdentifierNode("Value"));
									}
									codeStack.To(new InvocationExpressionBuilder("ISO8601String"));
								}
							} else if (finalType.SpecialType == SpecialType.System_String) {
								codeStack.With(new IdentifierNode(variableName));
							} else {
								codeStack.Begin().With(new IdentifierNode(variableName))
									.To(new StringInterpolationBuilder())
								.End();
							}
						}
					}
				}
			}
			if (type.IsNullable()) { codeStack.End(); }
			return codeStack;
		}
		object IConvertObject<ControllerInfo>.Convert(ControllerInfo from) {
			return Convert(from);
		}
	}
}

