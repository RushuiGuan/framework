using Microsoft.CodeAnalysis.CSharp;

namespace Albatross.CodeAnalysis.Syntax {
	public class BaseTypeNode : NodeContainer {
		public BaseTypeNode(string name)
			: base(SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(name))) {
		}
	}
}