using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Messaging.CodeGen {
	public class CommandInterfaceImplementationWalker : CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		private readonly string interfaceName;

		public List<INamedTypeSymbol> Results { get; } = new List<INamedTypeSymbol>();

		public CommandInterfaceImplementationWalker(SemanticModel semanticModel, string interfaceName) {
			this.semanticModel = semanticModel;
			this.interfaceName = interfaceName;
		}
		public override void VisitRecordDeclaration(RecordDeclarationSyntax node) {
			Find(node);
			base.VisitRecordDeclaration(node);
		}

		public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
			Find(node);
			base.VisitClassDeclaration(node);
		}

		void Find(SyntaxNode node) {
			var symbol = semanticModel.GetDeclaredSymbol(node) as INamedTypeSymbol;
			if (symbol != null && !symbol.IsAbstract && symbol.AllInterfaces.Any(x => x.Name == interfaceName)) {
				Results.Add(symbol);
			}
		}
	}
}