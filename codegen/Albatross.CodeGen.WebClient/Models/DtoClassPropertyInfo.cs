using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.WebClient.Models {
	public record class DtoClassPropertyInfo {
		public DtoClassPropertyInfo(IPropertySymbol symbol) {
			this.Name = symbol.Name;
			this.Type = symbol.Type;
		}

		public string Name { get; set; }
		[JsonIgnore]
		public ITypeSymbol Type { get; set; }
		public string TypeName => Type.GetFullName();
	}
}
