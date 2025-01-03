using Albatross.CodeAnalysis.Symbols;
using Albatross.Collections;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeGen.WebClient.Models {
	public record class DtoClassInfo {
		public DtoClassInfo(INamedTypeSymbol symbol) {
			this.Name = symbol.Name;
			var properties = new Dictionary<string, DtoClassPropertyInfo>();
			symbol.AllInterfaces.ForEach(x => BaseTypes.Add(x.GetFullName()));

			while (symbol != null) {
				foreach (var item in symbol.GetMembers().OfType<IPropertySymbol>()
					.Where(x => !(symbol.IsRecord && x.Name == "EqualityContract"))
					.Select(x => new DtoClassPropertyInfo(x))) {

					if (!item.Symbol.TryGetAttribute("System.Text.Json.Serialization.JsonIgnoreAttribute", out _)) {
						if (!properties.ContainsKey(item.Name)) {
							properties.Add(item.Name, item);
						}
					}
				}
				if (symbol.BaseType != null && symbol.BaseType.SpecialType != SpecialType.System_Object) {
					BaseTypes.Add(symbol.BaseType.GetFullName());
					symbol = symbol.BaseType;
				} else {
					symbol = null;
				}
			}
			Properties = properties.Values.ToArray();
		}

		public string Name { get; }
		public DtoClassPropertyInfo[] Properties { get; }
		public HashSet<string> BaseTypes { get; } = new HashSet<string>();
	}
}