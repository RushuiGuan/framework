using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.WebClient.Models {
	public record class DtoClassInfo {
		public DtoClassInfo(INamedTypeSymbol symbol) {
			this.Name = symbol.Name;
			Properties = symbol.GetMembers().OfType<IPropertySymbol>()
				.Where(x => !(symbol.IsRecord && x.Name == "EqualityContract"))
				.Select(x => new DtoClassPropertyInfo(x)).ToArray();

			if (symbol.BaseType != null && symbol.BaseType.SpecialType != SpecialType.System_Object) {
				BaseType = symbol.BaseType;
			} else {
				BaseType = null;
			}
		}

		public string Name { get; }
		[JsonIgnore]
		public INamedTypeSymbol? BaseType { get; }
		public DtoClassPropertyInfo[] Properties { get; }
	}
}
