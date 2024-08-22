using Microsoft.CodeAnalysis;

namespace Albatross.CodeAnalysis {
	public interface INodeContainer : INode {
		SyntaxNode Node { get; }
	}
}
