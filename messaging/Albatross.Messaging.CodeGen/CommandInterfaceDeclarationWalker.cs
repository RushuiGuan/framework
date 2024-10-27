using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Albatross.Messaging.CodeGen {
	public record class CommandHandlerSetup {
		public CommandHandlerSetup(INamedTypeSymbol commandHandler, ITypeSymbol commandType) {
			CommandHandler = commandHandler;
			CommandType = commandType;
		}
		public INamedTypeSymbol CommandHandler {
			get; set;
		}
		public ITypeSymbol CommandType {
			get; set;
		}
		public ITypeSymbol? ReturnType {
			get; set;
		}
	}
	public class CommandInterfaceDeclarationWalker : CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		private readonly Regex regex = new Regex("^I[a-zA-Z0-9_]*Command$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
		public HashSet<INamedTypeSymbol> FoundInterfaces { get; } = [];
		public Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> FoundImplementations { get; } = [];
		public List<CommandHandlerSetup> CommandHandlers { get; } = [];

		public CommandInterfaceDeclarationWalker(SemanticModel semanticModel) {
			this.semanticModel = semanticModel;
		}

		bool IsEligibleCommandInterface(INamedTypeSymbol symbol) {
			if (FoundInterfaces.Contains(symbol) || FoundImplementations.ContainsKey(symbol)) {
				return true;
			}
			if (symbol.TypeKind == TypeKind.Interface && symbol.IsPartial()) {
				if (symbol.GetAttributes().Any(x => x.AttributeClass?.Name.EndsWith("CommandInterfaceAttribute") == true)) {
					return true;
				} else if (regex.IsMatch(symbol.Name) && symbol.GetMembers().IsEmpty) {
					return true;
				}
			}
			return false;
		}

		public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node) {
			if (node.Modifiers.Any(SyntaxKind.PartialKeyword)) {
				var interfaceSymbol = semanticModel.GetDeclaredSymbol(node);
				if (interfaceSymbol != null) {
					if (IsEligibleCommandInterface(interfaceSymbol)) {
						FoundInterfaces.Add(interfaceSymbol!);
					}
				}
			}
			base.VisitInterfaceDeclaration(node);
		}

		public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
			var classSymbol = semanticModel.GetDeclaredSymbol(node);
			if (classSymbol != null && !classSymbol.IsAbstract) {
				foreach (var interfaceSymbol in classSymbol.AllInterfaces) {
					if (IsEligibleCommandInterface(interfaceSymbol)) {
						if (!FoundImplementations.TryGetValue(interfaceSymbol, out var classes)) {
							classes = new List<INamedTypeSymbol>();
							FoundImplementations.Add(interfaceSymbol, classes);
						}
						classes.Add(classSymbol);
						break;
					} else if (interfaceSymbol.IsGenericType) {
						if (interfaceSymbol.OriginalDefinition.GetFullName() == "Albatross.Messaging.Commands.ICommandHandler<>") {
							CommandHandlers.Add(new CommandHandlerSetup(classSymbol, interfaceSymbol.TypeArguments[0]));
							break;
						} else if (interfaceSymbol.OriginalDefinition.GetFullName() == "Albatross.Messaging.Commands.ICommandHandler<,>") {
							CommandHandlers.Add(new CommandHandlerSetup(classSymbol, interfaceSymbol.TypeArguments[0]) {
								ReturnType = interfaceSymbol.TypeArguments[1]
							});
							break;
						}
					}
				}
			}
		}
	}
}