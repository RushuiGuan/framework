using Albatross.CodeAnalysis;
using Albatross.CodeGen.CSharp;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Albatross.CodeGen.TypeScript;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Albatross.CodeGen.WebClient.CSharp;
using Albatross.CodeGen.WebClient.TypeScript;
using Microsoft.CodeAnalysis;
using System;
using Albatross.Text;
using Microsoft.AspNetCore.Components.Forms.Mapping;

namespace Albatross.CodeGen.WebClient {
	public static class Extensions {
		public static string GetHttpMethod(this IMethodSymbol methodSymbol) {
			var httpMethodAttribute = methodSymbol.GetAttributes().Where(attrib => attrib.AttributeClass?.BaseType?.GetFullName() == My.HttpMethodAttributeClassName)
				.Select(attrib => attrib.AttributeClass).FirstOrDefault() ?? throw new InvalidOperationException($"Method {methodSymbol.Name} is missing http method attribute");
			return httpMethodAttribute.Name.ToLower().TrimStart("http").TrimEnd("attribute");
		}
		public static string GetRoute(this ISymbol symbol) {
			var route = string.Empty;
			if (symbol.TryGetAttribute(My.RouteAttributeClassName, out var routeAttribute)) {
				route = routeAttribute!.ConstructorArguments.FirstOrDefault().Value as string ?? string.Empty;
			}
			if (!string.IsNullOrEmpty(route)) {
				return route;
			}
			foreach (var attributeData in symbol.GetAttributes()) {
				if (attributeData.AttributeClass?.BaseType.GetFullName() == My.HttpMethodAttributeClassName){
					route = attributeData.ConstructorArguments.FirstOrDefault().Value as string;
					if (!string.IsNullOrEmpty(route)) { return route; }
				}
			}
			return string.Empty;
		}

		public static string GetMethod(this HttpMethodAttribute attribute) {
			if (attribute is HttpPatchAttribute) {
				return "GetPatchMethod()";
			} else {
				string text = attribute.HttpMethods.First();
				return $"HttpMethod.{text.Substring(0, 1)}{text.Substring(1).ToLower()}";
			}
		}
		public static IServiceCollection AddWebClientCodeGen(this IServiceCollection services) {
			services.AddCSharpCodeGen().AddTypeScriptCodeGen();
			services.TryAddSingleton<ICreateApiCSharpProxy, CreateApiCSharpProxy>();
			services.TryAddSingleton<ICreateAngularPublicApi, CreateAngularPublicApi>();
			services.TryAddScoped<ICreateWebClientMethod, CreateWebClientMethod>();
			services.TryAddSingleton<ConvertApiControllerToCSharpClass>();
			services.TryAddSingleton<ConvertApiControllerToTypeScriptFile>();
			services.TryAddScoped<ConvertApiControllerToTypeScriptFile>();
			return services;
		}
	}
}
