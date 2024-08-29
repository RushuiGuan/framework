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
	public class VariableBuilder : INodeBuilder {

		public VariableBuilder(string? type, string name) {
			type = string.IsNullOrEmpty(type) ? "var" : type;
			this.Node = SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(type!));
			this.Name = name;
		}

		public VariableDeclarationSyntax Node { get; private set; }
		public string Name { get; }

		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			if (elements.Count() <= 1) {
				if (elements.FirstOrDefault() is ExpressionSyntax expressionSyntax) {
					this.Node = this.Node.WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(this.Name)
						.WithInitializer(SyntaxFactory.EqualsValueClause(expressionSyntax))));
				} else {
					this.Node = this.Node.WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(this.Name)));
				}
				return this.Node;
			} else {
				throw new ArgumentException($"LocalVariable declaration takes one parameter of type ExpressionSyntax");
			}
		}
	}
}
