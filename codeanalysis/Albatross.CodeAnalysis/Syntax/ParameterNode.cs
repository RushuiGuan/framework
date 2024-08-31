using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeAnalysis.Syntax {
	public class ParameterNode : INodeContainer {
		public ParameterNode(string type, string name) : this(false, type, name) { }
		public ParameterNode(bool withThis, string type, string name) {
			var paramNode = SyntaxFactory.Parameter(SyntaxFactory.Identifier(name))
				.WithType(SyntaxFactory.ParseTypeName(type));
			if (withThis) {
				paramNode = paramNode.AddModifiers(SyntaxFactory.Token(SyntaxKind.ThisKeyword));
			}
			this.Node = paramNode;
		}

		public ParameterSyntax Parameter => (ParameterSyntax)Node;
		public SyntaxNode Node { get; }
	}
}