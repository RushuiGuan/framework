using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Albatross.CodeGen.WebClient.Models {
	public static class Extensions {
		public static string GetRouteText(this ISymbol symbol) {
			var attribute = symbol.GetAttributes().FirstOrDefault(x => x.AttributeClass?.GetFullName() == My.RouteAttributeClassName);
			if (attribute == null) {
				attribute = symbol.GetAttributes().FirstOrDefault(x => x.AttributeClass?.BaseType?.GetFullName() == My.HttpMethodAttributeClassName);
			}
			if (attribute == null) {
				return string.Empty;
			} else {
				return attribute.ConstructorArguments.FirstOrDefault().Value as string ?? string.Empty;
			}
		}

		public readonly static Regex RouteTemplateRegex = new Regex(@"{(\*){0,2}([a-z_]+[a-z0-9_]*)}", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
		public static IEnumerable<IRouteSegment> GetRouteSegments(this string routeTemplate) {
			int pos = 0;
			for (Match match = RouteTemplateRegex.Match(routeTemplate); match.Success; match = match.NextMatch()) {
				yield return new RouteTextSegment(routeTemplate.Substring(pos, match.Index - pos));
				yield return new RouteParameterSegment(match.Groups[2].Value);
				pos = match.Index + match.Length;
			}
			if (pos < routeTemplate.Length - 1) {
				yield return new RouteTextSegment(routeTemplate.Substring(pos));
			}
		}
	}
}