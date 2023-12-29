using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using Albatross.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Albatross.CodeGen.WebClient {
	public class ConvertApiControllerToTypeScriptClass : IConvertObject<Type, Class> {
		public string? EndpointName { get; set; }
		const string Controller = "Controller";
		private readonly IConvertObject<MethodInfo, Method> convertMethod;
		private readonly IConvertObject<Type, TypeScriptType> typeConverter;
		Regex actionRouteRegex = new Regex(@"{(\w+)}", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);

		public ConvertApiControllerToTypeScriptClass(IConvertObject<MethodInfo, Method> convertMethod, IConvertObject<Type, TypeScriptType> typeConverter) {
			this.convertMethod = convertMethod;
			this.typeConverter = typeConverter;
		}

		void CreateConstructor(Class model) {
			model.Constructor = new Constructor() {
				Parameters = new ParameterDeclaration[] {
					new ParameterDeclaration("config", new TypeScriptType("ConfigService"), AccessModifier.Private),
					new ParameterDeclaration("client", new TypeScriptType("HttpClient"), AccessModifier.Protected),
					new ParameterDeclaration("logger", new TypeScriptType("Logger"), AccessModifier.None),
				},
			};
			model.Constructor.Body.Add(new Super(new TypeScriptValue(TypeScript.Model.ValueType.Variable, "logger")));
			model.Constructor.Body.Add(new Termination());
			model.Constructor.Body.Add(new LoggerInfo($"{model.Name} instance created"));
			model.Constructor.Body.Add(new Termination());
		}

		void CreateImport(Class model) {
			model.Imports.Add(new Import("@angular/common/http", "HttpClient"));
			model.Imports.Add(new Import("@angular/core", "Injectable"));
		}

		void CreateEndPointGetter(Class model, Type type, string endPointName) {
			var getter = new Getter("endPoint", AccessModifier.None, TypeScriptType.String());
			getter.Body.Add($"return this.config.endpoint('{endPointName}') + '{this.GetControllerRoute(type)}/'");
			model.Getters.Add(getter);
		}

		public Class Convert(Type type) {
			Class model = new Class($"{GetController(type)}Service") {
				BaseClass = new Class("DataService"),
				Decorator = new InjectableDecorator("root")
			};
			CreateConstructor(model);
			CreateImport(model);
			if (string.IsNullOrEmpty(this.EndpointName)) {
				throw new InvalidOperationException("Endpoint has not been initialized for ConvertApiControllerToTypeScriptClass");
			}
			CreateEndPointGetter(model,type, this.EndpointName);
			List<Method> list = new List<Method>();
			ISet<string> names = new HashSet<string>();
			foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)) {
				foreach (var attrib in methodInfo.GetCustomAttributes()) {
					if (attrib is HttpMethodAttribute) {
						var method = GetMethod((HttpMethodAttribute)attrib, methodInfo);
						var originalName = method.Name;
						int index = 1;
						while (names.Contains(method.Name)) {
							method.Name = $"{originalName}{index}";
							index++;
						}
						names.Add(method.Name);
						list.Add(method);
						break;
					}
				}
			}
			model.Methods.AddRange(list);
			return model;
		}

		string GetController(Type type) {
			if (type.Name.EndsWith(Controller)) {
				return type.Name.Substring(0, type.Name.Length - Controller.Length);
			} else {
				return type.Name;
			}
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

		Method GetMethod(HttpMethodAttribute attrib, MethodInfo methodInfo) {
			Method method = convertMethod.Convert(methodInfo);
			// javascript can't handle datetime correctly, might as well just use text instead
			// ideally it should be formatted to sometime of date string
			foreach (var param in method.Parameters) {
				if (object.Equals(param.Type, TypeScriptType.Date())) { 
					param.Type = TypeScriptType.String(); 
				}
			}
			method.Async = true;
			// fix method return types
			// all api calls are returning Promises
			method.ReturnType = TypeScriptType.MakeAsync(method.ReturnType);

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
			var relativeUrl = new VariableDeclaration("relativeUrl", true, null);
			relativeUrl.Assignment = new StringInterpolation(route.Replace("{", "${"));
			method.Body.Add(relativeUrl);

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

			var array = queryParams.Select(args => {
				var attribute = args.GetCustomAttribute<FromQueryAttribute>();
				return new TypeScriptValue(TypeScript.Model.ValueType.Variable, attribute?.Name ?? args.Name!.CamelCase());
			}).ToArray();
			var queryParamValue = new TypeScriptValueArray(array);

			if (attrib is HttpGetAttribute) {
				if (method.ReturnType.GenericTypeArguments.First().Equals(TypeScriptType.String())) {
					var call = new MethodCall(true, "this.doGetStringAsync", new TypeScriptValue(TypeScript.Model.ValueType.Variable, relativeUrl.Name), queryParamValue);
					var resultVariable = new VariableDeclaration("result", true, null);
					resultVariable.Assignment = call;
					method.Body.Add(resultVariable);
				} else {
					var call = new MethodCall(true, "this.doGetAsync", new TypeScriptValue(TypeScript.Model.ValueType.Variable, relativeUrl.Name), queryParamValue);
					call.GenericArguments.Add(method.ReturnType.GenericTypeArguments.First());
					var resultVariable = new VariableDeclaration("result", true, null);
					resultVariable.Assignment = call;
					method.Body.Add(resultVariable);
				}
				method.Body.Add("return result;");
			} else if (attrib is HttpPostAttribute) {
				if (method.ReturnType.GenericTypeArguments.First().Equals(TypeScriptType.String())) {
					var call = new MethodCall(true, "this.doPostStringAsync", new TypeScriptValue(TypeScript.Model.ValueType.Variable, relativeUrl.Name), new TypeScriptValue(TypeScript.Model.ValueType.Variable, bodyParam!.Name!), queryParamValue);
					call.GenericArguments.Add(method.ReturnType.GenericTypeArguments.First());
					if (bodyParam == null) {
						call.GenericArguments.Add(TypeScriptType.Any());
					} else {
						call.GenericArguments.Add(this.typeConverter.Convert(bodyParam.ParameterType));
					}
					var resultVariable = new VariableDeclaration("result", true, null);
					resultVariable.Assignment = call;
					method.Body.Add(resultVariable);
				} else {
					var call = new MethodCall(true, "this.doPostAsync",
						new TypeScriptValue(TypeScript.Model.ValueType.Variable, relativeUrl.Name),
						new TypeScriptValue(TypeScript.Model.ValueType.Variable, bodyParam?.Name),
						queryParamValue);
					call.GenericArguments.Add(method.ReturnType.GenericTypeArguments.First());
					if (bodyParam == null) {
						call.GenericArguments.Add(TypeScriptType.Any());
					} else {
						call.GenericArguments.Add(typeConverter.Convert(bodyParam.ParameterType));
					}
					var resultVariable = new VariableDeclaration("result", true, null);
					resultVariable.Assignment = call;
					method.Body.Add(resultVariable);
				}
				method.Body.Add("return result;");
			} else if (attrib is HttpDeleteAttribute) {
				var call = new MethodCall(true, "this.doDeleteAsync", new TypeScriptValue(TypeScript.Model.ValueType.Variable, relativeUrl.Name), queryParamValue);
				method.Body.Add(call);
			}
			return method;
		}
	}
}
