using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Declarations;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertMethodParameter : IConvertObject<IParameterSymbol, ParameterDeclaration> {
		private readonly IConvertObject<ITypeSymbol, ITypeExpression> typeConverter;

		public ConvertMethodParameter(IConvertObject<ITypeSymbol, ITypeExpression> typeConverter) {
			this.typeConverter = typeConverter;
		}

		public ParameterDeclaration Convert(IParameterSymbol from) {
			return new ParameterDeclaration(from.Name) {
				Type = typeConverter.Convert(from.Type),
			};
		}

		object IConvertObject<IParameterSymbol>.Convert(IParameterSymbol from) => this.Convert(from);
	}
}
