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

			var names = new HashSet<string>();
			var defaultCommandName = commandType.GetFullName();
			foreach (var attribute in commandType.GetAttributes()) {
				if (attribute.AttributeClass?.GetFullName() == "Albatross.Messaging.Core.CommandNameAttribute") {
					defaultCommandName = attribute.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? defaultCommandName;
				} else if (attribute.AttributeClass?.GetFullName() == "Albatross.Messaging.Core.AlternateCommandNameAttribute") {
					var name = attribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
					if (!string.IsNullOrEmpty(name)) {
						names.Add(name!);
					}
				}
			}
			names.Add(defaultCommandName);
			this.CommandNames = names;
		}

		public IEnumerable<string> CommandNames { get; }
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
	/// <summary>
	/// A single walker class that will grab all the information needed to generate the messaging code.
	/// Command Interfaces
	/// it will find any interfaces with the following criterias:
	/// 1. partial modifier
	/// 2. name ends with "Command"
	/// 3. has no members
	/// or
	/// 1. partial modifier
	/// 2. has an attribute with the name CommandInterfaceAttribute.  Namespace of the attribute doesn't matter.
	/// Note that CommandInterfaceAttribute doesn't exist.  It can be created by the consumer of the generated code.
	/// 
	/// Command Interface Implementations
	/// It will find all the implementations of the interfaces found above
	/// 
	/// Command Handlers
	/// It will find all the class that implements `Albatross.Messaging.Commands.ICommandHandler<>` and `Albatross.Messaging.Commands.ICommandHandler<,>`
	/// </summary>
	public class MessagingCodeGenSyntaxWalker : CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		private readonly Regex regex = new Regex("^I[a-zA-Z0-9_]*Command$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
		public Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>> CommandInterfaces { get; } = [];
		public List<CommandHandlerSetup> CommandHandlers { get; } = [];

		public MessagingCodeGenSyntaxWalker(SemanticModel semanticModel) {
			this.semanticModel = semanticModel;
		}

		bool IsEligibleCommandInterface(INamedTypeSymbol symbol) {
			if (CommandInterfaces.ContainsKey(symbol)) {
				return true;
			}
			if (symbol.TypeKind == TypeKind.Interface && symbol.DeclaringSyntaxReferences.Any() && symbol.IsPartial()) {
				if (symbol.GetAttributes().Any(x => x.AttributeClass?.Name == "CommandInterfaceAttribute")) {
					return true;
				} else if (regex.IsMatch(symbol.Name) && symbol.GetMembers().IsEmpty) {
					return true;
				}
			}
			return false;
		}

		public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
			var classSymbol = semanticModel.GetDeclaredSymbol(node);
			VisitClassDeclaration(classSymbol);
			base.VisitClassDeclaration(node);
		}
		public override void VisitRecordDeclaration(RecordDeclarationSyntax node) {
			base.VisitRecordDeclaration(node);
			var classSymbol = semanticModel.GetDeclaredSymbol(node);
			VisitClassDeclaration(classSymbol);
			base.VisitRecordDeclaration(node);
		}

		void VisitClassDeclaration(INamedTypeSymbol? classSymbol) {
			if (classSymbol != null && classSymbol.IsConcreteClass()) {
				// logger.WriteLine($"check class: {classSymbol.GetFullName()}");
				foreach (var interfaceSymbol in classSymbol.AllInterfaces) {
					if (IsEligibleCommandInterface(interfaceSymbol)) {
						if (!CommandInterfaces.TryGetValue(interfaceSymbol, out var classes)) {
							classes = new List<INamedTypeSymbol>();
							CommandInterfaces.Add(interfaceSymbol, classes);
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