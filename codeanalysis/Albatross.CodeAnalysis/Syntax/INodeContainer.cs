using Microsoft.CodeAnalysis;

namespace Albatross.CodeAnalysis.Syntax {
	public interface INodeContainer : INode {
		SyntaxNode Node { get; }
	}
}
