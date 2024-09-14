using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.WebClient.Models {
	public record class MethodInfo {
		private readonly Compilation compilation;

		public MethodInfo(Compilation compilation, IMethodSymbol symbol) {
			this.compilation = compilation;
			this.Name = symbol.Name;
			this.ReturnType = GetReturnType((INamedTypeSymbol)symbol.ReturnType);
			this.Route = symbol.GetRoute();
			if(!string.IsNullOrEmpty(this.Route) && !this.Route.StartsWith("/")) {
				this.Route = "/" + this.Route;
			}
			this.HttpMethod = GetHttpMethod(symbol);
			foreach(var parameter in symbol.Parameters) {
				this.Parameters.Add(new ParameterInfo(parameter, this.Route));
			}
		}
		public string HttpMethod { get; set; }
		public string Name { get; set; }
		[JsonIgnore]
		public INamedTypeSymbol ReturnType { get; set; }
		public string ReturnTypeText => ReturnType.GetFullName();
		public string Route { get; set; }
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
						return "get";
					case My.HttpPostAttributeClassName:
						return "post";
					case My.HttpPutAttributeClassName:
						return "put";
					case My.HttpDeleteAttributeClassName:
						return "delete";
					case My.HttpPatchAttributeClassName:
						return "patch";
				}
			}
			return string.Empty;
		}
	}
}
