using Albatross.CodeGen.CSharp.Models;
using Albatross.Reflection;
using Albatross.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Albatross.CodeGen.WebClient.CSharp {
	public class ConvertApiControllerToCSharpClass : IConvertObject<Type, Class> {
		const string Controller = "Controller";
		const string ProxyService = "ProxyService";
		const string WebClient = "WebClient";
		const string ControllerPath = "ControllerPath";
		const string VariableName_Logger = "logger";
		const string VariableName_Client = "client";

		public readonly static Regex ActionRouteRegex = new Regex(@"{(\*\*){0,2}([a-z_]+[a-z0-9_]*)}", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private IConvertObject<MethodInfo, Method> convertMethod;

		public ConvertApiControllerToCSharpClass(IConvertObject<MethodInfo, Method> convertMethod) {
			this.convertMethod = convertMethod;
		}

		object IConvertObject<Type>.Convert(Type from) {
			return Convert(from);
		}

		IEnumerable<Parameter> GetConstructorParameters(Type type) {
			return new Parameter[]{
				new Parameter(VariableName_Logger, GetILoggerType(type)) {
					Modifier = CodeGen.CSharp.Models.ParameterModifier.None,
				},
				new Parameter(VariableName_Client, new DotNetType(typeof(HttpClient))){
					Modifier = CodeGen.CSharp.Models.ParameterModifier.None,
				}
			};
		}
		IEnumerable<Variable> GetBaseContructorCallVariables() {
			return new Variable[]{
				new Variable(VariableName_Logger, true),
				new Variable(VariableName_Client, true),
				new Variable("Albatross.Serialization.DefaultJsonSettings.Value", false),
			};
		}

		public Class Convert(Type type) {
			var converted = new Class(GetClassName(type)) {
				Partial = true,
				Imports = new string[] { "System", "System.Net.Http", "System.Threading.Tasks", "Microsoft.Extensions.Logging", "Albatross.WebClient", "System.Collections.Generic", "Albatross.Serialization" },
				AccessModifier = AccessModifier.Public,
				Namespace = GetNamespace(type),
				BaseClass = GetBaseClass(),
				Fields = new Field[] {
					new Field(ControllerPath, DotNetType.String()) {
						Const = true,
						Value = new StringWriter().Literal(GetControllerRoute(type)).ToString(),
						Modifier = AccessModifier.Public,
					},
				},
				Constructors = new Constructor[] {
					new Constructor(GetClassName(type)){
						AccessModifier = AccessModifier.Public,
						Parameters = GetConstructorParameters(type),
						BaseConstructor = new MethodCall("base"){
							Parameters = GetBaseContructorCallVariables(),
						}
					}
				},
			};
			var list = new List<Method>();
			foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.Instance)) {
				foreach (var attrib in methodInfo.GetCustomAttributes()) {
					if (attrib is HttpMethodAttribute) {
						list.Add(GetMethod((HttpMethodAttribute)attrib, methodInfo));
						break;
					}
				}
			}
			converted.Methods = list.ToArray();
			// Auto-generated code requires an explicit '#nullable' directive in source.
			converted.UseNullablePreprocessor = true;
			return converted;
		}

		DotNetType GetILoggerType(Type type) {
			return new DotNetType("Microsoft.Extensions.Logging.ILogger", false, false, new DotNetType[0]);
		}

		DotNetType GetGenericILoggerType(Type type) {
			return new DotNetType("Microsoft.Extensions.Logging.ILogger", false, true, new DotNetType[] { new DotNetType(GetClassName(type)), });
		}

		string GetControllerName(Type type) {
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
					list[i] = GetControllerName(type);
				}
			}
			return string.Join("/", list).ToLower();
		}

		string GetNamespace(Type type) {
			string[] list = type.Namespace?.Split('.') ?? new string[1];
			list[list.Length - 1] = WebClient;
			return string.Join(".", list);
		}

		string GetClassName(Type type) {
			return GetControllerName(type) + ProxyService;
		}

		Class GetBaseClass() {
			return new Class("Albatross.WebClient.ClientBase");
		}

		string ActionResultClassName => typeof(ActionResult).FullName ?? throw new Exception();
		string TaskClassName => typeof(Task).FullName ?? throw new Exception();

		bool TryGetGenericTypeArgument(DotNetType type, string genericTypeName, [NotNullWhen(true)] out DotNetType? genericType) {
			if (type.IsGeneric && type.Name == genericTypeName && type.GenericTypeArguments.Count() == 1) {
				genericType = type.GenericTypeArguments[0];
				return true;
			} else {
				genericType = null;
				return false;
			}
		}

		Method GetMethod(HttpMethodAttribute attrib, MethodInfo methodInfo) {
			string name = methodInfo.Name;
			Method method = convertMethod.Convert(methodInfo);

			if (TryGetGenericTypeArgument(method.ReturnType, TaskClassName, out var returnType)) {
				method.ReturnType = returnType;
			}

			// first deal with ActionResult return types
			if (TryGetGenericTypeArgument(method.ReturnType, ActionResultClassName, out returnType)) {
				method.ReturnType = returnType;
			}

			/// make async void void and the rest async
			if (!method.ReturnType.IsAsync && !method.ReturnType.IsVoid) {
				method.ReturnType = DotNetType.MakeAsync(method.ReturnType);
			}

			string? actionTemplate = attrib.Template;
			if (string.IsNullOrEmpty(actionTemplate)) {
				actionTemplate = methodInfo.GetCustomAttribute<RouteAttribute>()?.Template;
			}

			StringBuilder sb = new StringBuilder();
			using (StringWriter writer = new StringWriter(sb)) {
				new AddCSharpRouteUrl(actionTemplate).Generate(writer);
				writer.WriteLine();
				writer.Code(new NewObjectCodeBlock<NameValueCollection>("queryString"));
				HashSet<string> actionRoutes = new HashSet<string>();
				if (attrib.Template != null) {
					foreach (Match match in ActionRouteRegex.Matches(attrib.Template)) {
						actionRoutes.Add(match.Groups[2].Value);
					}
				}

				ParameterInfo? fromBody = null;
				foreach (var item in methodInfo.GetParameters()) {
					if (item.Name != null) {
						if (item.GetCustomAttribute<FromBodyAttribute>() != null) {
							// skip the FromBody parameter
							fromBody = item;
						} else if (item.GetCustomAttribute<FromRouteAttribute>() != null || actionRoutes.Contains(item.Name)) {
							// skip the routing parameter
							continue;
						} else if (item.Name != null) {
							var attribute = item.GetCustomAttribute<FromQueryAttribute>();
							if (item.ParameterType.GetCollectionElementType(out Type? elementType)) {
								writer.Code(new ForEachCodeBlock("item", item.Name) {
									ForEachContent = new AddCSharpQueryStringParam(attribute?.Name ?? item.Name, "item", elementType)
								});
							} else {
								writer.Code(new AddCSharpQueryStringParam(attribute?.Name ?? item.Name, item.Name, item.ParameterType));
							}
						}
					}
				}
				if (fromBody?.ParameterType == typeof(string)) {
					writer.WriteLine($"using (var request = this.CreateStringRequest({attrib.GetMethod()}, path, queryString, @{fromBody.Name})) {{");
				} else if (fromBody == null) {
					writer.WriteLine($"using (var request = this.CreateRequest({attrib.GetMethod()}, path, queryString)) {{");
				} else {
					writer.WriteLine($"using (var request = this.CreateJsonRequest<{new DotNetType(fromBody.ParameterType)}>({attrib.GetMethod()}, path, queryString, @{fromBody.Name})) {{");
				}
				writer.Tab();
				//skip "return" if return type is void or Task
				if (!method.ReturnType.IsVoid) { writer.Write("return "); }

				if (!method.ReturnType.IsVoid && !method.ReturnType.Equals(new DotNetType(typeof(string))) && !method.ReturnType.Equals(new DotNetType(typeof(Task<string>)))) {
					if (method.ReturnType.IsNullableReferenceType || method.ReturnType.IsNullableValueType
						|| method.ReturnType.IsAsync && method.ReturnType.GenericTypeArguments[0].IsNullableReferenceType
						|| method.ReturnType.IsAsync && method.ReturnType.GenericTypeArguments[0].IsNullableValueType) {
						writer.Write($"await this.GetJsonResponse<{method.ReturnType.RemoveAsync().RemoveNullable()}>");
					} else {
						writer.Write($"await this.GetRequiredJsonResponse<{method.ReturnType.RemoveAsync()}>");
					}
				} else {
					writer.Write($"await this.GetRawResponse");
				}
				writer.WriteLine("(request);");
				writer.Write("}");
			}
			method.CodeBlock = new CodeBlock(sb.ToString());
			method.Async = true;
			return method;
		}
	}
}
