using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.Syntax {
	public interface ISourceLookup {
		bool TryGet(ITypeSymbol name, [NotNullWhen(true)] out ISourceExpression? module);
	}
}