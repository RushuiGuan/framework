using System.Collections.Generic;

namespace Albatross.CodeGen.Syntax {
	public interface IDeclaration : ISyntaxNode {
		public IEnumerable<IModifier> Modifiers { get; }
	}
}