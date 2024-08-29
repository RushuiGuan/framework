using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Generate a <see cref="InvocationExpressionSyntax"/> instance.  Will take one optional <see cref="ArgumentListSyntax"/> as parameter.
	/// </summary>
	public class InvocationExpressionBuilder : INodeBuilder {
		public InvocationExpressionBuilder(params string[] names) : this(false, names) { }

		public InvocationExpressionBuilder(bool memberAccess, params string[] names) {
			Node = SyntaxFactory.InvocationExpression(new IdentifierNode(memberAccess, names).Identifier);
		}

		public InvocationExpressionSyntax Node { get; private set; }

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			if (elements.Count() <= 1) {
				var element = elements.FirstOrDefault();
				if (element != null) {
					if (element is ArgumentListSyntax argumentListSyntax) {
						return Node.WithArgumentList(argumentListSyntax);
					} else {
						throw new ArgumentException($"The {nameof(InvocationExpressionBuilder)} only accepts {nameof(ArgumentListSyntax)} as a parameter");
					}
				} else {
					return Node.WithArgumentList(SyntaxFactory.ArgumentList());
				}
			} else {
				throw new ArgumentException($"The {nameof(InvocationExpressionBuilder)} only accepts at most one {nameof(ArgumentListSyntax)} parameter");
			}
		}
	}
}
