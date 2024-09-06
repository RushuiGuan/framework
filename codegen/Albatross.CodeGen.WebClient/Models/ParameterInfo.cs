using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Albatross.CodeGen.WebClient.Models {
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum ParameterType {
		FromBody,
		FromQuery,
		FromRoute,
	}
	public record class ParameterInfo {
		public ParameterInfo(IParameterSymbol symbol, string route) {
			this.Name = symbol.Name;
			this.Type = symbol.Type;
			if (symbol.TryGetAttribute(My.FromBodyAttributeClassName, out var attribute)) {
				this.WebType = ParameterType.FromBody;
			} else if (symbol.TryGetAttribute(My.FromRouteAttributeClassName, out attribute)) {
				this.WebType = ParameterType.FromRoute;
			} else if (symbol.TryGetAttribute(My.FromQueryAttributeClassName, out attribute)) {
				this.WebType = ParameterType.FromQuery;
				if (attribute!.TryGetNamedArgument("Name", out var name)) {
					this.QueryKey = System.Convert.ToString(name.Value) ?? string.Empty;
				} else {
					this.QueryKey = this.Name;
				}
			} else {
				var pattern = $"{{\\**{this.Name}}}";
				var match = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase).Match(route);
				if (match.Success) {
					this.WebType = ParameterType.FromRoute;
				} else {
					this.WebType = ParameterType.FromQuery;
					this.QueryKey = this.Name;
				}
			}
		}
		public string QueryKey { get; set; } = string.Empty;
		public string Name { get; set; }
		[JsonIgnore]
		public ITypeSymbol Type { get; set; }
		public string TypeText => Type.GetFullName();
		public ParameterType WebType { get; set; }
	}
}
