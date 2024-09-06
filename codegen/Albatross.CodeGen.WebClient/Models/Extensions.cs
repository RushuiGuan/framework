using System.Linq;
using Microsoft.CodeAnalysis;
using Albatross.CodeAnalysis.Symbols;

namespace Albatross.CodeGen.WebClient.Models {
	public static class Extensions {
		public static string GetRoute(this ISymbol symbol) {
			var attribute = symbol.GetAttributes().FirstOrDefault(x => x.AttributeClass?.GetFullName() == My.RouteAttributeClassName);
			if(attribute == null) {
				attribute = symbol.GetAttributes().FirstOrDefault(x => x.AttributeClass?.BaseType?.GetFullName() == My.HttpMethodAttributeClassName);
			}
			if(attribute == null) {
				return string.Empty;
			}else{
				return attribute.ConstructorArguments.FirstOrDefault().Value as string ?? string.Empty;
			}
		}
	}
}
