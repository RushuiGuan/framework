using Albatross.CodeGen.TypeScript.Declarations;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertClassToTypeScriptInterface : IConvertObject<INamedTypeSymbol, InterfaceDeclaration> {
		public InterfaceDeclaration Convert(INamedTypeSymbol from) {
			return new InterfaceDeclaration(from.Name) {
			};
		}

		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) {
			return Convert(from);
		}
	}
}