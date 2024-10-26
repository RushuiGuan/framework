using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Albatross.CodeAnalysis.Syntax {
	public interface INode { }
	public interface INodeBuilder : INode {
		SyntaxNode Build(IEnumerable<SyntaxNode> elements);
	}
	public interface INodeContainer : INode {
		SyntaxNode Node { get; }
	}
}