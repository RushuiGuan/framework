using System.Collections.Generic;

namespace Albatross.CodeGen.Syntax {
	public interface ISyntaxNode : ICodeElement {
		IEnumerable<ISyntaxNode> GetDescendants();
	}
}