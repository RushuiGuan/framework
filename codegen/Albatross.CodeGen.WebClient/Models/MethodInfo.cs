using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient.Models {
	public record class MethodInfo {
		public MethodInfo(IMethodSymbol symbol) {
			this.Name = symbol.Name;
			this.ReturnType = GetReturnType((INamedTypeSymbol)symbol.ReturnType);
			this.Route = symbol.GetRoute();
			foreach(var parameter in symbol.Parameters) {
				this.Parameters.Add(new ParameterInfo(parameter));
			}
		}

		public string HttpMethod { get; set; }
		public string Name { get; set; }
		public string ReturnType { get; set; }
		public string Route { get; set; }
		public List<ParameterInfo> Parameters { get; } = new List<ParameterInfo>();

		string GetReturnType(INamedTypeSymbol type) {
			if (type.IsGenericType) {
				var genericTypeFullName = type.OriginalDefinition.GetFullName();
				switch (genericTypeFullName) {
					case My.GenericTaskClassName:
					case My.GenericActionResultClassName:
						return GetReturnType((INamedTypeSymbol)type.TypeArguments[0]);
					default:
						return type.GetFullName();
				}
			} else {
				var typeFullName = type.GetFullName();
				switch (typeFullName) {
					case My.TaskClassName:
					case My.ActionResultClassName:
					case My.ActionResultInterfaceName:
						return "void";
					default:
						return typeFullName;
				}
			}
		}
	}
}
