using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Albatross.CodeGen.Utility {
	public class ApiControllerClassWalker : CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		public List<INamedTypeSymbol> Result { get; } = new List<INamedTypeSymbol>();

		public ApiControllerClassWalker(SemanticModel semanticModel) {
			this.semanticModel = semanticModel;
		}
		public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
			if (node.Identifier.Text.EndsWith("Controller")) {
				var symbol = semanticModel.GetDeclaredSymbol(node);
				if (symbol?.BaseType != null && symbol.BaseType.Name == "ControllerBase") {
					Result.Add(symbol);
				}
			}
			base.VisitClassDeclaration(node);
		}
	}
}
