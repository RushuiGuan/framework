using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Generate a <see cref="InvocationExpressionSyntax"/> instance.  Expects the following parameters
	/// * <see cref="ExpressionSyntax"/> Required if no identifier is provided.
	/// * <see cref="ArgumentListSyntax"/> Optional.
	/// </summary>
	public class InvocationExpressionBuilder : INodeBuilder {
		ExpressionSyntax? identifier;
		public InvocationExpressionBuilder() { }
		public InvocationExpressionBuilder(string name) : this(new IdentifierNode(name)) { }
		public InvocationExpressionBuilder(IdentifierNode identifier) {
			this.identifier = identifier.Identifier;
		}

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			ArgumentListSyntax? argumentList = null;
			foreach(var element in elements) {
				if (element is ArgumentListSyntax argumentListSyntax) {
					if (argumentList == null) {
						argumentList = argumentListSyntax;
					} else {
						throw new ArgumentException($"The {nameof(InvocationExpressionBuilder)} only accepts at most one {nameof(ArgumentListSyntax)} parameter");
					}
				} else if (element is ExpressionSyntax expressionSyntax) {
					if (this.identifier == null) {
						this.identifier = expressionSyntax;
					} else {
						throw new ArgumentException($"The {nameof(InvocationExpressionBuilder)} only accepts one {nameof(ExpressionSyntax)} parameter");
					}
				} else {
					throw new ArgumentException($"The {nameof(InvocationExpressionBuilder)} only accepts {nameof(ExpressionSyntax)} and {nameof(ArgumentListSyntax)} as parameters");
				}
			}
			if (this.identifier == null) {
				throw new ArgumentException($"The {nameof(InvocationExpressionBuilder)} requires an {nameof(ExpressionSyntax)} parameter");
			} else {
				var node = SyntaxFactory.InvocationExpression(this.identifier);
				if (argumentList != null) {
					node = node.WithArgumentList(argumentList);
				}else{
					node = node.WithArgumentList(SyntaxFactory.ArgumentList());
				}
				return node;
			}
		}
	}
}
