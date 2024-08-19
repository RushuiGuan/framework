using Albatross.CodeAnalysis;
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
				if (classSymbol != null && classSymbol.TryGetAttribute(My.VerbAttributeClass, out _)) {
					Result.Add(classSymbol);
				}
			}
			base.VisitClassDeclaration(node);
		}
		public override void VisitRecordDeclaration(RecordDeclarationSyntax node) {
			if (node.Modifiers.Any(SyntaxKind.PublicKeyword)) {
				var classSymbol = semanticModel.GetDeclaredSymbol(node);
				if (classSymbol != null && classSymbol.TryGetAttribute(My.VerbAttributeClass, out _)) {
					Result.Add(classSymbol);
				}
			}
			base.VisitRecordDeclaration(node);
		}
	}
}
