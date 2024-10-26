using Albatross.CodeGen.Syntax;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public interface ITypeConverter {
		public int Precedence { get; }
		bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression);
	}
}