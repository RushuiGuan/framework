using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeAnalysis.Syntax {
	public class ParameterNode : INodeContainer {
		public ParameterNode(bool withThis, TypeSyntax typeSyntax, string name) {
			var paramNode = SyntaxFactory.Parameter(SyntaxFactory.Identifier(name))
				.WithType(typeSyntax);
			if (withThis) {
				paramNode = paramNode.AddModifiers(SyntaxFactory.Token(SyntaxKind.ThisKeyword));
			}
			this.Node = paramNode;
		}
		public ParameterNode(string type, string name) : this(false, type, name) { }
		public ParameterNode(bool withThis, string type, string name) : this(withThis, SyntaxFactory.ParseTypeName(type), name) { }
		public ParameterSyntax Parameter => (ParameterSyntax)Node;
		public SyntaxNode Node { get; }
	}
}