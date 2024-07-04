using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.Modifiers;
using Albatross.CodeGen.Syntax;
using Albatross.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Albatross.CodeGen.TypeScript;

namespace Albatross.CodeGen.WebClient {
	public class ConvertApiControllerToTypeScriptClass : IConvertObject<Type, ClassDeclaration> {
		public string? EndpointName { get; set; }
		const string Controller = "Controller";
		private readonly IConvertObject<Type, SimpleTypeExpression> typeConverter;
		private readonly IConvertObject<ParameterInfo, ParameterDeclaration> paramConverter;
		Regex actionRouteRegex = new Regex(@"{(\w+)}", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);

		public ConvertApiControllerToTypeScriptClass(IConvertObject<Type, SimpleTypeExpression> typeConverter, IConvertObject<ParameterInfo, ParameterDeclaration> paramConverter) {
			this.typeConverter = typeConverter;
			this.paramConverter = paramConverter;
		}

		string GetController(Type type) {
			if (type.Name.EndsWith(Controller)) {
				return type.Name.Substring(0, type.Name.Length - Controller.Length);
			} else {
				return type.Name;
			}
		}

		public ClassDeclaration Convert(Type type) {
			string className = $"{GetController(type)}Service";
			ClassDeclaration model = new ClassDeclaration(className) {
				BaseClassName = new QualifiedIdentifierNameExpression("WebClient", My.Sources.MirageWebClient),
				Decorator = Defined.Invocations.InjectableDecorator("root"),
				Constructor = new MethodDeclaration("constructor") {
					ReturnType = Defined.Types.Void,
					Parameters = new ListOfSyntaxNodes<ParameterDeclaration>(
						new ParameterDeclaration {
							Modifiers = [AccessModifier.Private],
							Identifier = new IdentifierNameExpression("config"),
							Type = new SimpleTypeExpression {
								Identifier = new QualifiedIdentifierNameExpression("ConfigService", My.Sources.MirageConfig),
							}
						},
						new ParameterDeclaration {
							Modifiers = [AccessModifier.Protected],
							Identifier = new IdentifierNameExpression("client"),
							Type = Defined.Types.HttpClient,
						},
						new ParameterDeclaration {
							Identifier = new IdentifierNameExpression("logger"),
							Type = My.Types.Logger,
						}),
					Body = new CompositeExpression(
						new InvocationExpression {
							Identifier = new IdentifierNameExpression("super"),
							ArgumentList = new ListOfSyntaxNodes<IExpression>(new IdentifierNameExpression("logger")),
						},
						new InvocationExpression {
							Identifier = new MultiPartIdentifierNameExpression(
									Defined.Identifiers.This,
									new IdentifierNameExpression("logger"),
									new IdentifierNameExpression("info")
								),
							ArgumentList = new ListOfSyntaxNodes<IExpression>(new StringLiteralExpression($"{className} instance created")),
						}
					),
				},
				Getters = [
					new GetterDeclaration("endPoint"){
						ReturnType = Defined.Types.String,
						Body = new ReturnExpression(
							new BinaryExpression {
								Left = new MultiPartIdentifierNameExpression("this", "config", "endpoint"),
								Operator = "+",
								Right = new StringLiteralExpression($"{this.GetControllerRoute(type)}/"),
							}
						),
					}
				],
				Methods = GetMethods(type),
			};
			return model;
		}

		IEnumerable<MethodDeclaration> GetMethods(Type type) {
			var list = new List<MethodDeclaration>();
			foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)) {
				foreach (var attrib in methodInfo.GetCustomAttributes()) {
					if (attrib is HttpMethodAttribute) {
						var method = GetMethod((HttpMethodAttribute)attrib, methodInfo);
						list.Add(method);
						break;
					}
				}
			}
			return list;
		}

		string GetControllerRoute(Type type) {
			RouteAttribute? route = type.GetCustomAttribute<RouteAttribute>();
			var list = route?.Template?.Split('/') ?? new string[0];
			for (int i = 0; i < list.Length; i++) {
				if (string.Equals(list[i], "[controller]")) {
					list[i] = GetController(type);
				}
			}
			return string.Join("/", list).ToLower();
		}

		MethodDeclaration GetMethod(HttpMethodAttribute attrib, MethodInfo methodInfo) {
			// figure out the relativeUrl
			string? route = attrib.Template;
			if (string.IsNullOrEmpty(route)) {
				route = methodInfo.GetCustomAttribute<RouteAttribute>()?.Template;
			}
			if (!string.IsNullOrEmpty(route)) {
				route = route.Replace("*", "");
			} else {
				route = string.Empty;
			}
			MethodDeclaration method = new MethodDeclaration(methodInfo.Name) {
				Parameters = new ListOfSyntaxNodes<ParameterDeclaration>(methodInfo.GetParameters().Select(x => paramConverter.Convert(x))),
				ReturnType = typeConverter.Convert(methodInfo.ReturnType),
				Body = new CompositeExpression {
					Items = [
						new LocalVariableExpression("relativeUrl"){
							Assignment = new StringInterpolationExpression(route.Replace("{", "${"))
						},
					],
				},
			};
			// figure out the routing params
			HashSet<string> actionRoutes = new HashSet<string>();
			if (attrib.Template != null) {
				foreach (Match match in actionRouteRegex.Matches(route)) {
					actionRoutes.Add(match.Groups[1].Value);
				}
			}

			List<ParameterInfo> queryParams = new List<ParameterInfo>();
			ParameterInfo? bodyParam = null;
			List<ParameterInfo> routingParams = new List<ParameterInfo>();

			foreach (var item in methodInfo.GetParameters()) {
				if (item.GetCustomAttribute<FromBodyAttribute>() != null) {
					bodyParam = item;
				} else if (item.GetCustomAttribute<FromRouteAttribute>() != null || actionRoutes.Contains(item.Name!)) {
					routingParams.Add(item);
				} else {
					queryParams.Add(item);
				}
			}

			var queryParamValue = new JsonObjectDeclaration();

			queryParams.ForEach(args => {
				var attribute = args.GetCustomAttribute<FromQueryAttribute>();
				queryParamValue.Add(attrib?.Name ?? args.Name!.CamelCase(), new IdentifierNameExpression(args.Name.CamelCase()));
			});

			if (attrib is HttpGetAttribute) {
				if (method.ReturnType.GenericTypeArguments.First().Equals(SimpleTypeExpression.String())) {
					var call = new MethodCallExpression(true, "this.doGetStringAsync", new TypeScriptValue(TypeScript.Models.ValueType.Variable, relativeUrl.Name), queryParamValue);
					var resultVariable = new VariableDeclaration("result", true, null);
					resultVariable.Assignment = call;
					method.Body.Add(resultVariable);
				} else {
					var call = new MethodCallExpression(true, "this.doGetAsync", new TypeScriptValue(TypeScript.Models.ValueType.Variable, relativeUrl.Name), queryParamValue);
					call.GenericArguments.Add(method.ReturnType.GenericTypeArguments.First());
					var resultVariable = new VariableDeclaration("result", true, null);
					resultVariable.Assignment = call;
					method.Body.Add(resultVariable);
				}
				method.Body.Add("return result;");
			} else if (attrib is HttpPostAttribute) {
				if (method.ReturnType.GenericTypeArguments.First().Equals(SimpleTypeExpression.String())) {
					var call = new MethodCallExpression(true, "this.doPostStringAsync", new TypeScriptValue(TypeScript.Models.ValueType.Variable, relativeUrl.Name), new TypeScriptValue(TypeScript.Models.ValueType.Variable, bodyParam!.Name!), queryParamValue);
					call.GenericArguments.Add(method.ReturnType.GenericTypeArguments.First());
					if (bodyParam == null) {
						call.GenericArguments.Add(SimpleTypeExpression.Any());
					} else {
						call.GenericArguments.Add(this.typeConverter.Convert(bodyParam.ParameterType));
					}
					var resultVariable = new VariableDeclaration("result", true, null);
					resultVariable.Assignment = call;
					method.Body.Add(resultVariable);
				} else {
					var call = new MethodCallExpression(true, "this.doPostAsync",
						new TypeScriptValue(TypeScript.Models.ValueType.Variable, relativeUrl.Name),
						new TypeScriptValue(TypeScript.Models.ValueType.Variable, bodyParam?.Name),
						queryParamValue);
					call.GenericArguments.Add(method.ReturnType.GenericTypeArguments.First());
					if (bodyParam == null) {
						call.GenericArguments.Add(SimpleTypeExpression.Any());
					} else {
						call.GenericArguments.Add(typeConverter.Convert(bodyParam.ParameterType));
					}
					var resultVariable = new VariableDeclaration("result", true, null);
					resultVariable.Assignment = call;
					method.Body.Add(resultVariable);
				}
				method.Body.Add("return result;");
			} else if (attrib is HttpDeleteAttribute) {
				var call = new MethodCallExpression(true, "this.doDeleteAsync", new TypeScriptValue(TypeScript.Models.ValueType.Variable, relativeUrl.Name), queryParamValue);
				method.Body.Add(call);
			}
			return method;
		}
	}
}
