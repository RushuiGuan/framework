using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Albatross.Messaging.CodeGen {
	public class CommandInterfaceDeclarationWalker : CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		private readonly Regex regex = new Regex("^I[a-zA-Z0-9_]*Command$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
		public List<INamedTypeSymbol> Result { get; } = new List<INamedTypeSymbol>();

		public CommandInterfaceDeclarationWalker(SemanticModel semanticModel) {
			this.semanticModel = semanticModel;
		}

		public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node) {
			if(node.Modifiers.Any(SyntaxKind.PartialKeyword)) {
				var interfaceSymbol = semanticModel.GetDeclaredSymbol(node) as INamedTypeSymbol;
				if (interfaceSymbol != null && regex.IsMatch(interfaceSymbol.Name) && interfaceSymbol.GetMembers().IsEmpty) {
					Result.Add(interfaceSymbol);
				}
			}
			base.VisitInterfaceDeclaration(node);
		}
	}
}
