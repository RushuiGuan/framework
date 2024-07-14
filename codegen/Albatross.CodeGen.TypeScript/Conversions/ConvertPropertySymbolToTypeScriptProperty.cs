using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.TypeConversions;
using Albatross.Text;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertPropertySymbolToTypeScriptProperty : IConvertObject<IPropertySymbol, PropertyDeclaration> {
		private readonly TypeConverterFactory converterFactory;

		public ConvertPropertySymbolToTypeScriptProperty(TypeConverterFactory converterFactory) {
			this.converterFactory = converterFactory;
		}

		public PropertyDeclaration Convert(IPropertySymbol from) {
			return new PropertyDeclaration(from.Name) {
				Type = converterFactory.Convert(from.Type),
			};
		}

		object IConvertObject<IPropertySymbol>.Convert(IPropertySymbol from)
			=>  this.Convert(from);
	}
}
