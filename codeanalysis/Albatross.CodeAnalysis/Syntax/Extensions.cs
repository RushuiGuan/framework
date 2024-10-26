using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	public static class Extensions {
		public static TypeNode AsTypeNode(this ITypeSymbol symbol) {
			if (symbol is IArrayTypeSymbol arrayType) {
				return new ArrayTypeNode(arrayType.ElementType.AsTypeNode());
			} else {
				var node = new TypeNode(symbol.GetFullName());
				if (symbol.IsNullableReferenceType()) {
					return node.NullableReferenceType();
				} else {
					return node;
				}
			}
		}
		public static BlockSyntax BlockSyntax(this IEnumerable<SyntaxNode> nodes)
			=> SyntaxFactory.Block(nodes.Select(x => new StatementNode(x).StatementSyntax));
	}
}