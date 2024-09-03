using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeGen.WebClient.Models {
	public record class WebApi {
		public const string ControllerNamePlaceholder = "[controller]";
		public const string ControllerPostfix = "Controller";
		public string ControllerName { get; set; }
		public string Route { get; set; }

		public List<MethodInfo> Methods { get; } = new List<MethodInfo>();

		public WebApi(INamedTypeSymbol symbol) {
			if (symbol.Name.EndsWith(ControllerPostfix)) {
				this.ControllerName = symbol.Name.Substring(0, symbol.Name.Length - ControllerPostfix.Length);
			} else {
				this.ControllerName = symbol.Name;
			}
			this.Route = symbol.GetRoute();
			this.Route = this.Route.Replace(ControllerNamePlaceholder, this.ControllerName.ToLower());

			foreach (var methodSymbol in symbol.GetMembers().OfType<IMethodSymbol>()) {
				if (methodSymbol.GetAttributes().Any(x => My.HttpMethodAttributeClasses.Contains(x.AttributeClass?.GetFullName()))) {
					Methods.Add(new MethodInfo(methodSymbol));
				}
			}
		}
	}
}
