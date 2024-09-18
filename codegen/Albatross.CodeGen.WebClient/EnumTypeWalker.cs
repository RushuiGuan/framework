using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.WebClient.Settings;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Albatross.CodeGen.WebClient {
	public class EnumTypeWalker : CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		public List<INamedTypeSymbol> Result { get; } = new List<INamedTypeSymbol>();
		private Settings.SymbolFilter filter;

		public EnumTypeWalker(SemanticModel semanticModel, SymbolFilterPatterns patterns) {
			this.semanticModel = semanticModel;
			filter = new Settings.SymbolFilter(patterns);
		}
		public override void VisitEnumDeclaration(EnumDeclarationSyntax node) {
			var symbol = semanticModel.GetDeclaredSymbol(node);
			if (symbol != null && symbol.DeclaredAccessibility == Accessibility.Public && filter.IsMatch(symbol.GetFullName())) {
				Result.Add(symbol);
			}
			base.VisitEnumDeclaration(node);
		}
	}
}
