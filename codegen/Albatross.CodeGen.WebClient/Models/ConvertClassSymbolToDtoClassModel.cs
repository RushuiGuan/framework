using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.WebClient.Models {
	public record class ConvertClassSymbolToDtoClassModel : IConvertObject<INamedTypeSymbol, DtoClassInfo> {
		public DtoClassInfo Convert(INamedTypeSymbol from) {
			return new DtoClassInfo(from);
		}
		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) => Convert(from);
	}
}