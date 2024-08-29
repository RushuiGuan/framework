using Microsoft.CodeAnalysis.CSharp;

namespace Albatross.CodeAnalysis {
	public class BaseTypeNode : NodeContainer {
		public BaseTypeNode(string name)
			: base(SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(name))) {
		}
	}
}
