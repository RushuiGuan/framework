using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeGen.WebClient.Models {
	public record class EnumInfo {
		public EnumInfo(INamedTypeSymbol symbol) {
			this.Name = symbol.Name;
			this.Members = symbol.GetMembers().OfType<IFieldSymbol>().Select(x => new EnumMember(x)).ToArray();
		}
		public string Name { get; }
		public IEnumerable<EnumMember> Members { get; }
	}
	public record class EnumMember {
		public EnumMember(IFieldSymbol symbol) {
			this.Name = symbol.Name;
			this.NumericValue = (long)(symbol.ConstantValue ?? 0);

		}
		public string Name { get; }
		public long NumericValue { get; }
		public bool UseTextAsValue { get; }
	}
}
