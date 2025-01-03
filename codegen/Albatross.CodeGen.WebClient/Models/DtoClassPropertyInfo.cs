using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.WebClient.Models {
	public record class DtoClassPropertyInfo {
		public DtoClassPropertyInfo(INamedTypeSymbol classType, IPropertySymbol symbol) {
			this.Class = classType;
			this.ClassName = classType.GetFullName();
			this.Name = symbol.Name;
			this.PropertyType = symbol.Type;
			this.PropertyTypeName = symbol.Type.GetFullName();
		}

		public string Name { get; }

		[JsonIgnore]
		public ITypeSymbol Class { get; }
		public string ClassName { get; }

		[JsonIgnore]
		public ITypeSymbol PropertyType { get; }
		public string PropertyTypeName { get; }
	}
}