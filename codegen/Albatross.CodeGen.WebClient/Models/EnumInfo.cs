using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeGen.WebClient.Models {
	public record class EnumInfo {
		public EnumInfo(INamedTypeSymbol symbol) {
			this.Name = symbol.Name;
			this.UseTextAsValue = false;
			if (symbol.TryGetAttribute(My.JsonConverterAttribute_ClassName, out var attributeData)) {
				if ((attributeData?.ConstructorArguments.FirstOrDefault().Value as ITypeSymbol)?.GetFullName() == My.JsonStringEnumConverter_ClassName) {
					this.UseTextAsValue = true;
				}
			}
			this.Members = symbol.GetMembers().OfType<IFieldSymbol>().Select(x => new EnumMember(x)).ToArray();
		}
		public string Name { get; }
		public IEnumerable<EnumMember> Members { get; }
		public bool UseTextAsValue { get; }
	}
	public record class EnumMember {
		public EnumMember(IFieldSymbol symbol) {
			this.Name = symbol.Name;
			this.NumericValue = System.Convert.ToInt64(symbol.ConstantValue);
		}
		public string Name { get; }
		public long NumericValue { get; }
	}
}