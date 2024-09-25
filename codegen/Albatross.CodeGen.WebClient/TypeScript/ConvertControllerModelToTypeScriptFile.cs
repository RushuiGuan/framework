using Humanizer;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.Modifiers;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;
using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.WebClient.Models;
using Albatross.Text;
using Albatross.CodeGen.WebClient.Settings;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public class ConvertControllerModelToTypeScriptFile : IConvertObject<ControllerInfo, TypeScriptFileDeclaration> {
		public const string ControllerPostfix = "Controller";
		public const string ControllerNamePlaceholder = "[controller]";
		private readonly TypeScriptWebClientSettings settings;
		private readonly IConvertObject<ITypeSymbol, ITypeExpression> typeConverter;

		public ConvertControllerModelToTypeScriptFile(CodeGenSettings settings, IConvertObject<ITypeSymbol, ITypeExpression> typeConverter) {
			this.settings = settings.TypeScriptWebClientSettings;
			this.typeConverter = typeConverter;
		}

		string ControllerName(INamedTypeSymbol controllerSymbol) {
			if (controllerSymbol.Name.EndsWith(ControllerPostfix)) {
				return controllerSymbol.Name.Substring(0, controllerSymbol.Name.Length - ControllerPostfix.Length);
			} else {
				throw new InvalidOperationException($"Controller class {controllerSymbol.GetFullName()} must be postfixed with {ControllerPostfix}");
			}
		}

		public TypeScriptFileDeclaration Convert(ControllerInfo model) {
			var fileName = $"{model.ControllerName.Kebaberize()}.generated.service";
			return new TypeScriptFileDeclaration(fileName) {
				ClasseDeclarations = [
					new ClassDeclaration($"{model.ControllerName}Service"){
						Decorators = [
							Defined.Invocations.InjectableDecorator("root"),
						],
						BaseClassName = new QualifiedIdentifierNameExpression(settings.BaseClassName, new ModuleSourceExpression(settings.BaseClassModule)),
						Getters = [
							new GetterDeclaration("endPoint"){
								ReturnType = Defined.Types.String(),
								Body = new ReturnExpression(new InfixExpression("+"){
									Left = new InvocationExpression{
										Identifier = new MultiPartIdentifierNameExpression("this", "config", "endpoint"),
										ArgumentList = new ListOfSyntaxNodes<IExpression>(new StringLiteralExpression(settings.EndPointName)),
									},
									Right = new StringLiteralExpression(model.Route),
								}),
							}
						],
						Constructor = new ConstructorDeclaration(){
							Parameters = new ListOfSyntaxNodes<ParameterDeclaration> {
								Nodes = [
									new ParameterDeclaration("config"){
										Type = new SimpleTypeExpression{
											Identifier =  new QualifiedIdentifierNameExpression("ConfigService", new ModuleSourceExpression(settings.ConfigServiceModule)),
										},
										Modifiers = [ AccessModifier.Private ],
									},
									new ParameterDeclaration("client"){
										Type = Defined.Types.HttpClient(),
										Modifiers = [ AccessModifier.Protected],
									}
								],
							},
							Body = new CompositeExpression{
								Items = [
									new InvocationExpression{
										Identifier = new IdentifierNameExpression("super"),
										Terminate = true,
									},
									Defined.Invocations.ConsoleLog($"{model.ControllerName}Service instance created"),
								]
							},
						},
						Methods = model.Methods.Select(x=>BuildMethod(x)).ToList()
					}
				],
			};
		}
		MethodDeclaration BuildMethod(MethodInfo method) {
			var returnType = this.typeConverter.Convert(method.ReturnType);
			if (object.Equals(returnType, Defined.Types.Void())) {
				returnType = Defined.Types.Object();
			}
			return new MethodDeclaration(method.Name.CamelCase()) {
				Modifiers = settings.UsePromise ? [new AsyncModifier()] : [],
				ReturnType = settings.UsePromise ? returnType.ToPromise() : returnType.ToObservable(),
				Parameters = new ListOfSyntaxNodes<ParameterDeclaration>(method.Parameters.Select(x => new ParameterDeclaration(x.Name) { Type = typeConverter.Convert(x.Type) })),
				Body = new ScopedVariableExpressionBuilder()
					.IsConstant()
					.WithName("relativeUrl").WithExpression(method.RouteTemplate.ConvertRoute2StringInterpolation())
					.Add(() => CreateHttpInvocationExpression(method))
					.BuildAll()
			};
		}
		IExpression CreateHttpInvocationExpression(MethodInfo method) {
			var builder = new InvocationExpressionBuilder();
			if (settings.UsePromise) {
				builder.Await();
			}
			var returnType = this.typeConverter.Convert(method.ReturnType);
			var hasVoidReturnType = false;
			if (object.Equals(returnType, Defined.Types.Void())) {
				hasVoidReturnType = true;
				returnType = Defined.Types.Object();
			}
			var hasStringReturnType = object.Equals(returnType, Defined.Types.String());
			switch (method.HttpMethod) {
				case My.HttpMethodGet:
					if (hasStringReturnType) {
						builder.WithMultiPartName("this", "doGetStringAsync");
					} else {
						builder.WithMultiPartName("this", "doGetAsync").AddGenericArgument(returnType);
					}
					break;
				case My.HttpMethodPost:
					if (hasStringReturnType) {
						builder.WithMultiPartName("this", "doPostStringAsync");
					} else {
						builder.WithMultiPartName("this", "doPostAsync");
						builder.AddGenericArgument(returnType);
					}
					break;
				case My.HttpMethodPatch:
					if (hasStringReturnType) {
						builder.WithMultiPartName("this", "doPatchStringAsync");
					} else {
						builder.WithMultiPartName("this", "doPatchAsync");
						builder.AddGenericArgument(returnType);
					}
					break;
				case My.HttpMethodPut:
					if (hasStringReturnType) {
						builder.WithMultiPartName("this", "doPutStringAsync");
					} else {
						builder.WithMultiPartName("this", "doPutAsync");
						builder.AddGenericArgument(returnType);
					}
					break;
				case My.HttpMethodDelete:
					builder.WithMultiPartName("this", "doDeleteAsync");
					break;
			}
			// add relativeUrl parameter
			builder.AddArgument(new IdentifierNameExpression("relativeUrl"));
			// add from body parameter if it exists
			var fromBodyParameter = method.Parameters.FirstOrDefault(x => x.WebType == ParameterType.FromBody);
			if (fromBodyParameter != null) {
				builder.AddGenericArgument(this.typeConverter.Convert(fromBodyParameter.Type));
				builder.AddArgument(new IdentifierNameExpression(fromBodyParameter.Name.CamelCase()));
			} else if (method.HttpMethod == My.HttpMethodPost || method.HttpMethod == My.HttpMethodPut || method.HttpMethod == My.HttpMethodPatch) {
				builder.AddGenericArgument(Defined.Types.String());
				builder.AddArgument(new StringLiteralExpression(""));
			}

			// build query string
			var queryParam = new JsonValueExpression(method.Parameters.Where(x => x.WebType == ParameterType.FromQuery)
				.Select(x => new JsonPropertyExpression(x.QueryKey, new IdentifierNameExpression(x.Name.CamelCase()))).ToArray());
			builder.AddArgument(queryParam);

			if (settings.UsePromise && hasVoidReturnType) {
				return builder.Build();
			} else {
				return new ScopedVariableExpressionBuilder()
					.IsConstant().WithName("result").WithExpression(builder.Build())
					.Add(() => new ReturnExpression(new IdentifierNameExpression("result")))
					.BuildAll();
			}
		}

		object IConvertObject<ControllerInfo>.Convert(ControllerInfo from) {
			return this.Convert(from);
		}
	}
}
