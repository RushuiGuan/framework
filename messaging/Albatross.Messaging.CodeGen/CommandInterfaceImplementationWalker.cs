using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Albatross.Messaging.CodeGen {
	public class CommandInterfaceImplementationWalker : CSharpSyntaxWalker {
		private readonly SemanticModel _semanticModel;
		private readonly string interfaceName;

		public List<INamedTypeSymbol> Results { get; } = new List<INamedTypeSymbol>();

		public CommandInterfaceImplementationWalker(SemanticModel semanticModel, string interfaceName) {
			_semanticModel = semanticModel;
			this.interfaceName = interfaceName;
		}

		public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
			// Get the class symbol from the semantic model
			var classSymbol = _semanticModel.GetDeclaredSymbol(node) as INamedTypeSymbol;
			if (classSymbol != null) {
				foreach (var implementedInterface in classSymbol.AllInterfaces) {
					if (implementedInterface.Name == interfaceName) {
						Results.Add(classSymbol);
						break;
					}
				}
			}
			base.VisitClassDeclaration(node);
		}
	}
}
