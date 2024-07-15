using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.TypeConversions;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertDtoClassToTypeScriptInterface : IConvertObject<INamedTypeSymbol, InterfaceDeclaration> {
		private readonly ITypeConverterFactory typeConverterFactory;
		private readonly IConvertObject<IPropertySymbol, PropertyDeclaration> propertyConverter;

		public ConvertDtoClassToTypeScriptInterface(ITypeConverterFactory typeConverterFactory, IConvertObject<IPropertySymbol, PropertyDeclaration> propertyConverter) {
			this.typeConverterFactory = typeConverterFactory;
			this.propertyConverter = propertyConverter;
		}

		public InterfaceDeclaration Convert(INamedTypeSymbol from) {
			return new InterfaceDeclaration(from.Name) {
				BaseInterfaceName = from.BaseType != null
					&& from.BaseType.GetFullName() != "System.Object"
					&& !from.BaseType.IsValueType ? typeConverterFactory.Convert(from.BaseType) : null,
				Properties = from.GetMembers().OfType<IPropertySymbol>()
					.Where(x => Filter(from, x))
					.Select(x => this.propertyConverter.Convert(x)).ToList(),
			};
		}

		bool Filter(INamedTypeSymbol typeSymbol, IPropertySymbol propertySymbol) {
			if (typeSymbol.IsRecord && propertySymbol.Name == "EqualityContract") {
				return false;
			}
			return propertySymbol.DeclaredAccessibility == Accessibility.Public;
		}

		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) {
			return Convert(from);
		}
	}
}