using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	public class ForEachStatementBuilder : INodeBuilder {
		private readonly string type;
		private readonly string name;
		private readonly string collection;

		public ForEachStatementBuilder(string? type, string name, string collection) {
			this.type = type ?? "var";
			this.name = name;
			this.collection = collection;
		}

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			return SyntaxFactory.ForEachStatement(SyntaxFactory.IdentifierName(type), name, SyntaxFactory.IdentifierName(collection),
				SyntaxFactory.Block(elements.Select(x => new StatementNode(x).StatementSyntax)));
		}
	}
}