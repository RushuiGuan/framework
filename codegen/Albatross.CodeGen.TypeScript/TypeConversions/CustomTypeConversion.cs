using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public interface ISourceLookup {
		bool TryGet(ITypeSymbol name, [NotNullWhen(true)] out ISourceExpression? module);
	}

	public class CustomTypeConversion: ITypeConverter {
		private readonly ISourceLookup sourceLookup;

		public CustomTypeConversion(ISourceLookup sourceLookup) {
			this.sourceLookup = sourceLookup;
		}

		public int Precedence => 999;
		public bool Match(ITypeSymbol symbol) => true;
		public ITypeExpression Convert(ITypeSymbol symbol, ITypeConverterFactory _) {
			if (sourceLookup.TryGet(symbol, out ISourceExpression source)) {
				return new SimpleTypeExpression { Identifier = new QualifiedIdentifierNameExpression(symbol.Name, source) };
			} else {
				return new SimpleTypeExpression { Identifier = new IdentifierNameExpression(symbol.Name) };
			}
		}
	}
}
