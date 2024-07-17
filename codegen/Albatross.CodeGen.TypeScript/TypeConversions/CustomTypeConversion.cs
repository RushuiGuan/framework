using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Conversions;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {

	public class CustomTypeConversion : ITypeConverter {
		private readonly ISourceLookup sourceLookup;

		public CustomTypeConversion(ISourceLookup sourceLookup) {
			this.sourceLookup = sourceLookup;
		}

		public int Precedence => 999;

		public bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression) {
			if (sourceLookup.TryGet(symbol, out ISourceExpression source)) {
				expression = new SimpleTypeExpression { Identifier = new QualifiedIdentifierNameExpression(symbol.Name, source) };
			} else {
				expression = new SimpleTypeExpression { Identifier = new IdentifierNameExpression(symbol.Name) };
			}
			return true;
		}
	}
}
