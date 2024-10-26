using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeAnalysis.Syntax {
	internal class NoOpNodeBuilder : INodeBuilder {
		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			throw new NotSupportedException();
		}
	}
}