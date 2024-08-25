using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis {
	/// <summary>
	/// Create a syntax node of <see cref="VariableDeclarationSyntax"/>.  It requires one parameter of type <see cref="ExpressionSyntax"/>.
	/// Since the variable is declare with `var` keyword and has to be initialized.
	/// </summary>
	public class LocalVariableBuilder : INodeBuilder {
		private readonly string name;

		public LocalVariableBuilder(string name) {
			this.name = name;
		}

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			if (elements.Count() == 1 && elements.First() is ExpressionSyntax expressionSyntax) {
				return SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("var"))
					.WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(this.name)
						.WithInitializer(SyntaxFactory.EqualsValueClause(expressionSyntax))
					));
			}
			throw new ArgumentException($"LocalVariable declaration takes one parameter of type ExpressionSyntax");
		}
	}
}
