using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.Models {
	public record class ConvertEnumSymbolToDtoEnumModel : IConvertObject<INamedTypeSymbol, EnumInfo> {
		public EnumInfo Convert(INamedTypeSymbol from) {
			return new EnumInfo(from);
		}
		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) => Convert(from);
	}
}