using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Build <see cref="AttributeArgumentListSyntax"/>
	/// Generate the argument list used by other syntaxes. Uses any number of parameters of type <see cref="ExpressionSyntax"/>.  Parameters are optional.
	/// <see cref="AssignmentExpressionSyntax"/> which is derived from <see cref="ExpressionSyntax"/> can be used to create named arguments.
	/// ```
	/// (1, 2, 3, Name = "a")
	/// ```
	/// </summary>
	public class AttributeArgumentListBuilder : INodeBuilder {
		IEnumerable<AttributeArgumentSyntax> Arguments(IEnumerable<SyntaxNode> nodes) {
			var items = nodes.OfType<ExpressionSyntax>().ToArray();
			foreach (var item in items) {
				yield return SyntaxFactory.AttributeArgument(item);
			}
		}

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var arguments = Arguments(elements);
			var result = SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList(arguments));
			return result;
		}
	}

}