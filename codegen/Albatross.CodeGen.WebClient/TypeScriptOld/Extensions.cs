using Microsoft.CodeAnalysis;
using System;
using System.Linq;
using Albatross.CodeAnalysis.Symbols;
using Albatross.Text;

namespace Albatross.CodeGen.WebClient.TypeScriptOld {
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
				if (attributeData.AttributeClass?.BaseType?.GetFullName() == My.HttpMethodAttributeClassName) {
					route = attributeData.ConstructorArguments.FirstOrDefault().Value as string;
					if (!string.IsNullOrEmpty(route)) { return route; }
				}
			}
			return string.Empty;
		}
	}
}
