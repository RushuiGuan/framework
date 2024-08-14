using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.Syntax {
	/// <summary>
	/// This interface is used to look up the typescript source for a given type symbol.  For example, when the codegen found a type 
	/// "Albatross.CodeGen.Tests.Dto.MyDto", it could lookup the source for that type and return the a FileNameSourceExpression of "./dto"
	/// An implementation of this interface could use a dictionary to map the namespace of the type to a source expression.
	/// </summary>
	public interface ISourceLookup {
		bool TryGet(ITypeSymbol name, [NotNullWhen(true)] out ISourceExpression? module);
	}
}