using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeAnalysis.Syntax {
	public class ParameterNode : INodeContainer {
		public ParameterNode(bool withThis, TypeNode typeNode, string name) {
			var paramNode = SyntaxFactory.Parameter(SyntaxFactory.Identifier(name))
				.WithType(typeNode.Type);
			if (withThis) {
				paramNode = paramNode.AddModifiers(SyntaxFactory.Token(SyntaxKind.ThisKeyword));
			}
			this.Node = paramNode;
		}
		public ParameterNode(TypeNode type, string name) : this(false, type, name) { }
		public ParameterNode(string type, string name) : this(false, type, name) { }
		public ParameterNode(bool withThis, string type, string name) : this(withThis, new TypeNode(type), name) { }

		public ParameterSyntax Parameter => (ParameterSyntax)Node;
		public SyntaxNode Node { get; }
	}
}