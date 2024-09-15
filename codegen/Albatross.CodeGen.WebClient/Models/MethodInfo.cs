using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.WebClient.Models {
	public record class MethodInfo {
		private readonly Compilation compilation;

		public MethodInfo(Compilation compilation, IMethodSymbol symbol) {
			this.compilation = compilation;
			this.Name = symbol.Name;
			this.ReturnType = GetReturnType((INamedTypeSymbol)symbol.ReturnType);
			var routeSegments = symbol.GetRouteText().GetRouteSegments().ToArray();
			this.HttpMethod = GetHttpMethod(symbol);
			foreach(var parameter in symbol.Parameters) {
				this.Parameters.Add(new ParameterInfo(parameter, routeSegments));
			}
			this.RouteTemplate = string.Join<IRouteSegment>("", routeSegments);
			if (!string.IsNullOrEmpty(this.RouteTemplate) && !this.RouteTemplate.StartsWith("/")) {
				this.RouteTemplate = "/" + this.RouteTemplate;
			}
		}
		public string HttpMethod { get; set; }
		public string Name { get; set; }
		[JsonIgnore]
		public INamedTypeSymbol ReturnType { get; set; }
		public string ReturnTypeText => ReturnType.GetFullName();
		public string RouteTemplate { get; set; }
		public List<ParameterInfo> Parameters { get; } = new List<ParameterInfo>();

		INamedTypeSymbol GetReturnType(INamedTypeSymbol type) {
			if (type.IsGenericType) {
				var genericTypeFullName = type.OriginalDefinition.GetFullName();
				switch (genericTypeFullName) {
					case My.GenericTaskClassName:
					case My.GenericActionResultClassName:
						return GetReturnType((INamedTypeSymbol)type.TypeArguments[0]);
					default:
						return type;
				}
			} else {
				var typeFullName = type.GetFullName();
				switch (typeFullName) {
					case My.TaskClassName:
					case My.ActionResultClassName:
					case My.ActionResultInterfaceName:
						return compilation.GetSpecialType(SpecialType.System_Void);
					default:
						return type;
				}
			}
		}
		string GetHttpMethod(IMethodSymbol symbol) {
			foreach (var attribute in symbol.GetAttributes()) {
				switch (attribute.AttributeClass?.GetFullName()) {
					case My.HttpGetAttributeClassName:
						return "Get";
					case My.HttpPostAttributeClassName:
						return "Post";
					case My.HttpPutAttributeClassName:
						return "Put";
					case My.HttpDeleteAttributeClassName:
						return "Delete";
					case My.HttpPatchAttributeClassName:
						return "Patch";
				}
			}
			return string.Empty;
		}
	}
}
