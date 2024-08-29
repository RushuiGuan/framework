using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.TypeConversions;
using Albatross.Text;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertClassProperty : IConvertObject<IPropertySymbol, PropertyDeclaration> {
		private readonly IConvertObject<ITypeSymbol, ITypeExpression> converterFactory;

		public ConvertClassProperty(IConvertObject<ITypeSymbol, ITypeExpression> converterFactory) {
			this.converterFactory = converterFactory;
		}

		public PropertyDeclaration Convert(IPropertySymbol from) {
			ITypeExpression type = converterFactory.Convert(from.Type);
			var optional = from.Type.IsNullable();
			return new PropertyDeclaration(from.Name.CamelCase()) {
				Type = type,
				Optional = optional,
			};
		}

		object IConvertObject<IPropertySymbol>.Convert(IPropertySymbol from)
			=>  this.Convert(from);
	}
}
