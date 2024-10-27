using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Albatross.Messaging.CodeGen {
	public class CommandInterfaceDeclarationWalker : CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		private readonly Regex regex = new Regex("^I[a-zA-Z0-9_]*Command$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
		public List<INamedTypeSymbol> FoundInterfaces { get; } = new List<INamedTypeSymbol>();
		public Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> FoundImplementations { get; } = [];

		public CommandInterfaceDeclarationWalker(SemanticModel semanticModel) {
			this.semanticModel = semanticModel;
		}

		bool IsEligibleInterface(INamedTypeSymbol? symbol) {
			if (symbol != null && symbol.TypeKind == TypeKind.Interface && symbol.IsPartial()) {
				if (symbol.GetAttributes().Any(x => x.AttributeClass?.Name.EndsWith("CommandInterfaceAttribute") == true)) {
					return true;
				} else if (symbol != null && regex.IsMatch(symbol.Name) && symbol.GetMembers().IsEmpty) {
					return true;
				}
			}
			return false;
		}

		public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node) {
			if (node.Modifiers.Any(SyntaxKind.PartialKeyword)) {
				var interfaceSymbol = semanticModel.GetDeclaredSymbol(node);
				if (IsEligibleInterface(interfaceSymbol)) {
					FoundInterfaces.Add(interfaceSymbol!);
				}
			}
			base.VisitInterfaceDeclaration(node);
		}

		public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
			var classSymbol = semanticModel.GetDeclaredSymbol(node);
			if (classSymbol != null && !classSymbol.IsAbstract) {
				var interfaceSymbol = classSymbol.AllInterfaces.FirstOrDefault(x => IsEligibleInterface(x));
				if (interfaceSymbol != null) {
					if (!FoundImplementations.TryGetValue(interfaceSymbol, out var classes)) {
						classes = new List<INamedTypeSymbol>();
						FoundImplementations.Add(interfaceSymbol, classes);
					}
					classes.Add(classSymbol);
				}
			}
		}
	}
}