using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.WebClient.Models {
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum ParameterType {
		FromBody,
		FromQuery,
		FromRoute,
	}
	public record class ParameterInfo {
		public ParameterInfo(IParameterSymbol symbol, IRouteSegment[] routeSegments) {
			this.Name = symbol.Name;
			this.Type = symbol.Type;
			if (symbol.TryGetAttribute(My.FromBodyAttributeClassName, out var attribute)) {
				this.WebType = ParameterType.FromBody;
			} else if (symbol.TryGetAttribute(My.FromRouteAttributeClassName, out attribute)) {
				this.WebType = ParameterType.FromRoute;
				var segment = FindSegment(Name, routeSegments);
				if (segment != null) {
					segment.ParameterInfo = this;
				}
			} else if (symbol.TryGetAttribute(My.FromQueryAttributeClassName, out attribute)) {
				this.WebType = ParameterType.FromQuery;
				if (attribute!.TryGetNamedArgument("Name", out var name)) {
					this.QueryKey = System.Convert.ToString(name.Value) ?? string.Empty;
				} else {
					this.QueryKey = this.Name;
				}
			} else {
				var segment = FindSegment(Name, routeSegments);
				if (segment != null) {
					this.WebType = ParameterType.FromRoute;
					segment.ParameterInfo = this;
				} else {
					this.WebType = ParameterType.FromQuery;
					this.QueryKey = this.Name;
				}
			}
		}

		RouteParameterSegment? FindSegment(string name, IRouteSegment[] segments) {
			foreach (var segment in segments) {
				if (segment is RouteParameterSegment parameterSegment
					&& string.Equals(parameterSegment.Text, name, System.StringComparison.InvariantCultureIgnoreCase)) {
					return parameterSegment;
				}
			}
			return null;
		}

		public string QueryKey { get; set; } = string.Empty;
		public string Name { get; set; }
		[JsonIgnore]
		public ITypeSymbol Type { get; set; }
		public string TypeText => Type.GetFullName();
		public ParameterType WebType { get; set; }

		//public bool IsCollection { get; set; }
		//public bool IsNullable{get;set; }
		//public bool IsValueType { get; set; }
	}
}