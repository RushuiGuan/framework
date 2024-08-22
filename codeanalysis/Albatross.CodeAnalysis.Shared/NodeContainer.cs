using Microsoft.CodeAnalysis;

namespace Albatross.CodeAnalysis {
	public class NodeContainer : INodeContainer {
		public NodeContainer(SyntaxNode node) {
			Node = node;
		}
		public SyntaxNode Node { get; protected set; }
		public override string ToString() => Node.NormalizeWhitespace(indentation: "\t").ToFullString();
	}
}
