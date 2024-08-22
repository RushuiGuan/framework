using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Albatross.CodeAnalysis {
	public interface INodeBuilder : INode {
		SyntaxNode Build(IEnumerable<SyntaxNode> elements);
	}
}
