using Albatross.CodeAnalysis;
using Humanizer;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.Modifiers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public class ConvertApiControllerToTypeScriptFile : IConvertObject<INamedTypeSymbol, TypeScriptFileDeclaration> {
		public const string ControllerPostfix = "Controller";
		public const string ControllerNamePlaceholder = "[controller]";
		private readonly TypeScriptWebClientSettings settings;
		private readonly ICreateWebClientMethod createWebClientMethod;
		private readonly Compilation compilation;

		public ConvertApiControllerToTypeScriptFile(TypeScriptWebClientSettings settings, ICreateWebClientMethod createWebClientMethod, Compilation compilation) {
			this.settings = settings;
			this.createWebClientMethod = createWebClientMethod;
			this.compilation = compilation;
		}
		string ControllerName(INamedTypeSymbol controllerSymbol) {
			if (controllerSymbol.Name.EndsWith(ControllerPostfix)) {
				return controllerSymbol.Name.Substring(0, controllerSymbol.Name.Length - ControllerPostfix.Length);
			} else {
				throw new InvalidOperationException($"Controller class {controllerSymbol.GetFullName()} must be postfixed with {ControllerPostfix}");
			}
		}
		string ControllerRoute(INamedTypeSymbol controllerSymbol) {
			var route = controllerSymbol.GetRoute();
			route = route?.Replace(ControllerNamePlaceholder, ControllerName(controllerSymbol).ToLower()) ?? string.Empty;
			if(!string.IsNullOrEmpty(route) && !route.EndsWith("/")){
				route = route + "/";
			}
			return route;
		}
		public TypeScriptFileDeclaration Convert(INamedTypeSymbol controllerSymbol) {
			var fileName = $"{ControllerName(controllerSymbol).Kebaberize()}.service";
			return new TypeScriptFileDeclaration(fileName) {
				ClasseDeclarations = [
					new ClassDeclaration($"{ControllerName(controllerSymbol)}Service"){
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
									Right = new StringLiteralExpression(ControllerRoute(controllerSymbol)),
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
									Defined.Invocations.ConsoleLog($"{ControllerName(controllerSymbol)}Service instance created"),
								]
							},
						},
						Methods = GetMethodSymbols(controllerSymbol).Select(x => createWebClientMethod.Convert(x)).ToList(),
					}
				],
			};
		}
		IEnumerable<IMethodSymbol> GetMethodSymbols(INamedTypeSymbol controllerSymbol) {
			return controllerSymbol.GetMembers().OfType<IMethodSymbol>()
				.Where(x => x.MethodKind == MethodKind.Ordinary && x.HasAttributeWithBaseType(My.HttpMethodAttributeClassName)).ToArray();
		}
		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) => Convert(from);
	}
}
