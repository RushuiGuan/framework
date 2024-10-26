using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Create a syntax node of <see cref="VariableDeclarationSyntax"/>.  It requires one parameter of type <see cref="ExpressionSyntax"/>.
	/// If the variable is declare with `var` keyword, it has to be initialized.
	/// </summary>
	public class VariableBuilder : INodeBuilder {

		public VariableBuilder(string? type, string name) {
			type = string.IsNullOrEmpty(type) ? "var" : type;
			Node = SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(type!));
			Name = name;
		}

		public VariableDeclarationSyntax Node { get; private set; }
		public string Name { get; }

		public virtual SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			if (elements.Count() <= 1) {
				if (elements.FirstOrDefault() is ExpressionSyntax expressionSyntax) {
					Node = Node.WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(Name)
						.WithInitializer(SyntaxFactory.EqualsValueClause(expressionSyntax))));
				} else {
					Node = Node.WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(Name)));
				}
				return Node;
			} else {
				throw new ArgumentException($"LocalVariable declaration takes one parameter of type ExpressionSyntax");
			}
		}
	}
}