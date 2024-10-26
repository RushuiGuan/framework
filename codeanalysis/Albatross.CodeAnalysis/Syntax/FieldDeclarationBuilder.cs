using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Albatross.CodeAnalysis.Syntax {
	public class FieldDeclarationBuilder : VariableBuilder {
		public FieldDeclarationBuilder(string type, string name) : base(type, name) {
			accessibility = SyntaxFactory.Token(SyntaxKind.PrivateKeyword);
		}

		SyntaxToken accessibility;
		SyntaxToken? constant;

		public override SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			VariableDeclarationSyntax declaration = (VariableDeclarationSyntax)base.Build(elements);
			var result = SyntaxFactory.FieldDeclaration(SyntaxFactory.List<AttributeListSyntax>(), SyntaxFactory.TokenList(accessibility), declaration);
			if (constant.HasValue) {
				result = result.AddModifiers(constant.Value);
			}
			return result;
		}
		public FieldDeclarationBuilder Const() {
			this.constant = SyntaxFactory.Token(SyntaxKind.ConstKeyword);
			return this;
		}
		public FieldDeclarationBuilder Public() {
			this.accessibility = SyntaxFactory.Token(SyntaxKind.PublicKeyword);
			return this;
		}
		public FieldDeclarationBuilder Private() {
			this.accessibility = SyntaxFactory.Token(SyntaxKind.PrivateKeyword);
			return this;
		}
		public FieldDeclarationBuilder Protected() {
			this.accessibility = SyntaxFactory.Token(SyntaxKind.StaticKeyword);
			return this;
		}
	}
}