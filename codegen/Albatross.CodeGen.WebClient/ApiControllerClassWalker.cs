using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.WebClient.Settings;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using SymbolFilter = Albatross.CodeGen.WebClient.Settings.SymbolFilter;

namespace Albatross.CodeGen.WebClient {
	public class ApiControllerClassWalker : CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		public List<INamedTypeSymbol> Result { get; } = new List<INamedTypeSymbol>();
		SymbolFilter filter;

		public ApiControllerClassWalker(SemanticModel semanticModel, SymbolFilterPatterns patterns) {
			this.semanticModel = semanticModel;
			this.filter = new SymbolFilter(patterns);
		}
		public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
			if (node.Identifier.Text.EndsWith("Controller")) {
				var symbol = semanticModel.GetDeclaredSymbol(node);
				if (symbol?.BaseType != null && symbol.BaseType.Name == "ControllerBase") {
					if (filter.ShouldKeep(symbol.GetFullName())) {
						Result.Add(symbol);
					}
				}
			}
			base.VisitClassDeclaration(node);
		}
	}
}