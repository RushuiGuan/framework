using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.WebClient.Settings;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.WebClient.Models {
	public record class MethodInfo {
		private readonly Compilation compilation;

		public MethodInfo(WebClientMethodSettings settings, Compilation compilation, IMethodSymbol symbol) {
			this.Settings = settings;
			this.compilation = compilation;
			this.Name = symbol.Name;
			this.ReturnType = GetReturnType(symbol.ReturnType);
			// if the return type is string, it should not be nullable
			if (this.ReturnType.SpecialType == SpecialType.System_String && this.ReturnType.IsNullableReferenceType()) {
				this.ReturnType = compilation.GetSpecialType(SpecialType.System_String);
			}
			var routeSegments = symbol.GetRouteText().GetRouteSegments().ToArray();
			this.HttpMethod = GetHttpMethod(symbol);
			foreach (var parameter in symbol.Parameters) {
				this.Parameters.Add(new ParameterInfo(parameter, routeSegments));
			}
			this.RouteSegments = routeSegments;
		}
		public WebClientMethodSettings Settings { get; init; }
		public string HttpMethod { get; set; }
		public string Name { get; set; }
		[JsonIgnore]
		public ITypeSymbol ReturnType { get; set; }
		public string ReturnTypeText => ReturnType.GetFullName();
		public IEnumerable<IRouteSegment> RouteSegments { get; }
		public List<ParameterInfo> Parameters { get; } = new List<ParameterInfo>();

		ITypeSymbol GetReturnType(ITypeSymbol type) {
			if (type is INamedTypeSymbol named && named.IsGenericType) {
				var genericTypeFullName = named.OriginalDefinition.GetFullName();
				switch (genericTypeFullName) {
					case My.GenericTaskClassName:
					case My.GenericActionResultClassName:
						return GetReturnType(named.TypeArguments[0]);
					case My.AsyncEnumerableClassName:
						return compilation.GetSpecialType(SpecialType.System_Collections_Generic_IEnumerable_T)
							.Construct(named.TypeArguments[0]);
					default:
						return named;
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