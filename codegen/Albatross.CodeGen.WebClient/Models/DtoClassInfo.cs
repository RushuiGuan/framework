using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeGen.WebClient.Models {
	public record class DtoClassInfo {
		public DtoClassInfo(INamedTypeSymbol symbol) {
			this.Name = symbol.Name;
			var properties = new Dictionary<string, DtoClassPropertyInfo>();

			while (symbol != null) {
				foreach (var item in symbol.GetMembers().OfType<IPropertySymbol>()
				.Where(x => !(symbol.IsRecord && x.Name == "EqualityContract"))
				.Select(x => new DtoClassPropertyInfo(symbol, x))) {
					if (!properties.ContainsKey(item.Name)) {
						properties.Add(item.Name, item);
					}
				}
				if (symbol.BaseType != null && symbol.BaseType.SpecialType != SpecialType.System_Object) {
					symbol = symbol.BaseType;
				} else {
					symbol = null;
				}
			}
			Properties = properties.Values.ToArray();
		}

		public string Name { get; }
		public DtoClassPropertyInfo[] Properties { get; }
	}
}