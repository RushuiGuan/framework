using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.Messaging.CodeGen {
	[Generator]
	public class CommandInterfaceCodeGen : ISourceGenerator {
		public void Execute(GeneratorExecutionContext context) {
			// System.Diagnostics.Debugger.Launch();
			var compilation = context.Compilation;

			List<INamedTypeSymbol> cadidates = new List<INamedTypeSymbol>();

			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var interfaceWalker = new CommandInterfaceDeclarationWalker(semanticModel);
				interfaceWalker.Visit(syntaxTree.GetRoot());
				cadidates.AddRange(interfaceWalker.Result);
			}
			if (!cadidates.Any()) {
				string text = $"Please provide an interface with the partial modifier, without any members and with a name that matches the following regex: ^I[a-zA-Z0-9_]*Command$";
				var descriptor = new DiagnosticDescriptor("CmdInterfaceCodeGen01", "Command Interface CodeGen", text, "Generator", DiagnosticSeverity.Warning, true);
				context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
			} else {
				foreach (var candidate in cadidates) {
					var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(candidate.ContainingNamespace.ToDisplayString())).NormalizeWhitespace();

					var usings = new HashSet<string> { "System", "System.Text.Json.Serialization" };
					var declaration = SyntaxFactory.InterfaceDeclaration(candidate.Name)
						.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
						.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
					
					var typeDiscriminators = new List<string>();

					foreach (var syntaxTree in compilation.SyntaxTrees) {
						var semanticModel = compilation.GetSemanticModel(syntaxTree);
						var classWalker = new CommandInterfaceImplementationWalker(semanticModel, candidate.Name);
						classWalker.Visit(syntaxTree.GetRoot());


						foreach (var item in classWalker.Results) {
							if (!typeDiscriminators.Contains(item.Name)) {
								typeDiscriminators.Add(item.Name);
								usings.Add(item.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
								var firstArgument = SyntaxFactory.AttributeArgument(SyntaxFactory.TypeOfExpression(SyntaxFactory.ParseTypeName(item.Name)));
								var secondArgument = SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(item.Name)));
								var attributeArgumentList = SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList(new[] { firstArgument, secondArgument }));
								var generatedCodeAttribute = SyntaxFactory.Attribute(SyntaxFactory.ParseName("JsonDerivedType"), attributeArgumentList);
								var attributeList = SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(generatedCodeAttribute));
								declaration = declaration.AddAttributeLists(attributeList);
							} else {
								string text = $"Interface {candidate.Name} has two class implementations of the same name {item.Name}";
								var descriptor = new DiagnosticDescriptor("CmdInterfaceCodeGen02", "Command Interface CodeGen", text, "Generator", DiagnosticSeverity.Error, true);
								context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
							}
						}
					}
					var compilationUnit = SyntaxFactory.CompilationUnit();
					foreach (var item in usings) {
						compilationUnit = compilationUnit.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(item)));
					}
					@namespace = @namespace.AddMembers(declaration);
					compilationUnit = compilationUnit.AddMembers(@namespace);
					var code = compilationUnit.NormalizeWhitespace().ToFullString();
					context.AddSource(candidate.Name, SourceText.From(code, Encoding.UTF8));
				}
			}
		}

		public void Initialize(GeneratorInitializationContext context) {
		}
	}
}
