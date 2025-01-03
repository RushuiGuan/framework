using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.WebClient.Models {
	public record class DtoClassPropertyInfo {
		public DtoClassPropertyInfo(IPropertySymbol symbol) {
			this.Symbol = symbol;
			this.Name = symbol.Name;
			this.PropertyType = symbol.Type;
		}

		public string Name { get; }

		[JsonIgnore]
		public IPropertySymbol Symbol { get; }

		[JsonIgnore]
		public ITypeSymbol PropertyType { get; }
		public string PropertyTypeName => PropertyType.GetFullName();
	}
}