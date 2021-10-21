using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Albatross.CodeGen.WebClient {
	public class ConvertApiControllerToTypeScriptClass : IConvertObject<Type, Class> {
		const string Controller = "Controller";
		const string ProxyService = "ProxyService";
		const string WebClient = "WebClient";


		Regex actionRouteRegex = new Regex(@"{(\w+)}", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);


		public ConvertApiControllerToTypeScriptClass() {
		}

		object IConvertObject<Type>.Convert(Type from) {
			return this.Convert(from);
		}
		IEnumerable<Parameter> GetConstructorParameters(Type type) {
			return new Parameter[0];
		}

		public Class Convert(Type type) {
			Class converted = new Class(GetClassName(type)) {
				BaseClass = GetBaseClass(),
				Constructor = new Constructor {
					AccessModifier = AccessModifier.Public,
					Name = GetClassName(type),
					Parameters = GetConstructorParameters(type),
					BaseConstructor = new Constructor {
						Name = "base",
						Parameters = GetConstructorParameters(type),
					}
				}
			};
			List<Method> list = new List<Method>();
			foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.Instance)) {
				foreach (var attrib in methodInfo.GetCustomAttributes()) {
					if (attrib is HttpMethodAttribute) {
						list.Add(GetMethod((HttpMethodAttribute)attrib, methodInfo));
						break;
					}
				}
			}
			converted.Methods = list.ToArray();
			return converted;
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

		string GetNamespace(Type type) {
			string[] list = type.Namespace?.Split('.') ?? new string[0];
			list[list.Length - 1] = WebClient;
			return string.Join(".", list);
		}

		string GetClassName(Type type) {
			return GetController(type) + ProxyService;
		}

		Class GetBaseClass() {
			return new Class("Albatross.WebClient.ClientBase") {
			};
		}
		Method GetMethod(HttpMethodAttribute attrib, MethodInfo methodInfo) {
			string? actionTemplate = attrib.Template;
			if (string.IsNullOrEmpty(actionTemplate)) {
				actionTemplate = methodInfo.GetCustomAttribute<RouteAttribute>()?.Template;
			}
			Method method = new Method(methodInfo.Name);
			/// make async void void and the rest async
			if (!method.ReturnType.IsAsync && !method.ReturnType.IsVoid) {
				method.ReturnType = TypeScriptType.MakeAsync(method.ReturnType);
			}
			StringBuilder sb = new StringBuilder();
			using (StringWriter writer = new StringWriter(sb)) {
				writer.Write("string path = $\"{ControllerPath}");
				if (!string.IsNullOrEmpty(actionTemplate)) {
					writer.Write("/");
					writer.Write(actionTemplate);
				}
				writer.WriteLine("\";");
				writer.WriteLine("var queryString = new System.Collections.Specialized.NameValueCollection();");
				HashSet<string> actionRoutes = new HashSet<string>();
				if (attrib.Template != null) {
					foreach (Match match in actionRouteRegex.Matches(attrib.Template)) {
						actionRoutes.Add(match.Groups[1].Value);
					}
				}

				ParameterInfo? fromBody = null;
				foreach (var item in methodInfo.GetParameters()) {
					if (item.GetCustomAttribute<FromBodyAttribute>() != null) {
						fromBody = item;
					} else if (item.Name != null && !actionRoutes.Contains(item.Name)) {
						writer.Write($"queryString.Add(nameof(@{item.Name}), ");
						if (item.ParameterType == typeof(DateTime) || item.ParameterType == typeof(DateTime?)) {
							if (item.Name.EndsWith("date", StringComparison.InvariantCultureIgnoreCase)) {
								writer.WriteLine($"string.Format(\"{{0:yyyy-MM-dd}}\", @{item.Name}));");
							} else {
								writer.WriteLine($"string.Format(\"{{0:o}}\", @{item.Name}));");
							}
						} else if (item.ParameterType != typeof(string)) {
							writer.WriteLine($"System.Convert.ToString(@{item.Name}));");
						} else {
							writer.WriteLine($"@{item.Name});");
						}
					}
				}
				if (fromBody?.ParameterType == typeof(string)) {
					writer.WriteLine($"using (var request = this.CreateStringRequest({attrib.GetMethod()}, path, queryString, @{fromBody.Name})) {{");
				} else if (fromBody == null) {
					writer.WriteLine($"using (var request = this.CreateRequest({attrib.GetMethod()}, path, queryString)) {{");
				} else {
					writer.WriteLine($"using (var request = this.CreateJsonRequest<{new TypeScriptType(fromBody.ParameterType)}>({attrib.GetMethod()}, path, queryString, @{fromBody.Name})) {{");
				}
				writer.Tab();
				//skip "return" if return type is void or Task
				if (!method.ReturnType.IsVoid) { writer.Write("return "); }

				writer.Write($"await this.Invoke");
				if (!method.ReturnType.IsVoid && !method.ReturnType.Equals(new TypeScriptType(typeof(string))) && !method.ReturnType.Equals(new TypeScriptType(typeof(Task<string>)))) {
					writer.Write($"<{method.ReturnType.RemoveAsync()}>");
				}
				writer.WriteLine("(request);");
				writer.Write("}");
			}
			method.Body = new CodeBlock(sb.ToString());
			method.Async = true;
			return method;
		}
	}
}
