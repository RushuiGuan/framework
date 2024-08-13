using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Hosting.CommandLine.CodeGen {
	public class CommandOptionsDeclarationWalker : CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		public List<INamedTypeSymbol> Result { get; } = new List<INamedTypeSymbol>();

		public CommandOptionsDeclarationWalker(SemanticModel semanticModel) {
			this.semanticModel = semanticModel;
		}
		public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
			if (node.Modifiers.Any(SyntaxKind.PublicKeyword)) {
				var classSymbol = semanticModel.GetDeclaredSymbol(node);
				if (classSymbol != null && classSymbol.GetAttributes().Any(x => x.AttributeClass?.ToDisplayString() == "Albatross.Hosting.CommandLine.VerbAttribute")) {
					Result.Add(classSymbol);
				}
			}
			base.VisitClassDeclaration(node);
		}
	}
}
