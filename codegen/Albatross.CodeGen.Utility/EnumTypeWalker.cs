using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Albatross.CodeGen.Utility {
	public class EnumTypeWalker : CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		public List<INamedTypeSymbol> Result { get; } = new List<INamedTypeSymbol>();

		public EnumTypeWalker(SemanticModel semanticModel) {
			this.semanticModel = semanticModel;
		}
		public override void VisitEnumDeclaration(EnumDeclarationSyntax node) {
			var symbol = semanticModel.GetDeclaredSymbol(node);
			if(symbol != null && symbol.DeclaredAccessibility == Accessibility.Public) {
				Result.Add(symbol);
			}
			base.VisitEnumDeclaration(node);
		}
	}
}
