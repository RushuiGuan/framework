using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Albatross.CodeAnalysis.Syntax {
	public class ParameterNode : INodeContainer {
		public ParameterNode(string type, string name) {
			Node = SyntaxFactory.Parameter(SyntaxFactory.Identifier(name))
				.WithType(SyntaxFactory.ParseTypeName(type));
		}

		public SyntaxNode Node { get; }
	}
}