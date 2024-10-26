using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Generate the argument list used by other syntaxes. Uses any number of parameters of type <see cref="ExpressionSyntax"/>.  Parameters are optional.
	/// ```
	/// (1, 2, 3)
	/// ```
	/// </summary>
	public class ArgumentListBuilder : INodeBuilder {
		IEnumerable<ArgumentSyntax> Arguments(IEnumerable<SyntaxNode> nodes) {
			foreach (var node in nodes) {
				if (node is ExpressionSyntax expression) {
					yield return SyntaxFactory.Argument(expression);
				} else {
					throw new ArgumentException($"ArgumentListSyntax expects the ExpressionSyntax type as its parameters");
				}
			}
		}

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var arguments = Arguments(elements);
			var result = SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(arguments));
			return result;
		}
	}

}