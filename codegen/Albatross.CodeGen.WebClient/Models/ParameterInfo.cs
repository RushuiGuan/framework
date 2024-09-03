using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.Models {
	public enum ParameterType {
		FromBody,
		FromQuery,
		FromRoute,
	}
	public record class ParameterInfo {
		public ParameterInfo(IParameterSymbol symbol, string route) {
			this.Name = symbol.Name;
			this.Type = symbol.Type.GetFullName();
			foreach(var attribute in symbol.GetAttributes()) {
				var fullName = attribute.AttributeClass?.GetFullName();
				switch (fullName) {
					case My.FromBodyAttributeClassName:
						this.WebType = ParameterType.FromBody;
						break;
					case My.FromQueryAttributeClassName:
						this.WebType = ParameterType.FromQuery;
						break;
					case My.FromRouteAttributeClassName:
						this.WebType = ParameterType.FromRoute;
						break;
					default:
						this.WebType = route.Contains($"{{{this.Name}}}", System.StringComparison.InvariantCultureIgnoreCase) ? ParameterType.FromRoute : ParameterType.FromQuery;
						break;
				}
			}
		}
		public string Name { get; set; }
		public string Type { get; set; }
		public ParameterType WebType { get; set; }
	}
}
