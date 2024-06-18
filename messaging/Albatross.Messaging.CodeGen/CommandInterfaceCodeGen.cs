using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
			var rootNamespace = compilation.AssemblyName;

			if (rootNamespace != null) {
				var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(rootNamespace)).NormalizeWhitespace();
				INamedTypeSymbol? found = null;

				foreach (var syntaxTree in compilation.SyntaxTrees) {
					var semanticModel = compilation.GetSemanticModel(syntaxTree);
					var interfaceWalker = new CommandInterfaceDeclarationWalker(semanticModel);
					interfaceWalker.Visit(syntaxTree.GetRoot());

					found = interfaceWalker.Result.FirstOrDefault();
					if (found != null) {
						break;
					}
				}
				if (found == null) {
					string text = $"Please provide an interface with partial modifier that matches the following regex: ^I[a-zA-Z0-9_]*Command$";
					var descriptor = new DiagnosticDescriptor("CmdInterfaceCodeGen01", "Command Interface CodeGen", text, "Generator", DiagnosticSeverity.Warning, true);
					var warning = Diagnostic.Create(descriptor, Location.None);
					context.ReportDiagnostic(warning);
				} else {
					var usings = new HashSet<string> { "System", "System.Text.Json.Serialization" };
					var declaration = SyntaxFactory.InterfaceDeclaration(found.Name)
						.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
						.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
					int counter = 1;
					foreach (var syntaxTree in compilation.SyntaxTrees) {
						var semanticModel = compilation.GetSemanticModel(syntaxTree);
						var classWalker = new CommandInterfaceImplementationWalker(semanticModel, found.Name);
						classWalker.Visit(syntaxTree.GetRoot());


						foreach (var item in classWalker.Results) {
							usings.Add(item.ContainingNamespace.ToDisplayString());
							var firstArgument = SyntaxFactory.AttributeArgument(SyntaxFactory.TypeOfExpression(SyntaxFactory.ParseTypeName(item.Name)));
							var secondArgument = SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(counter)));
							var attributeArgumentList = SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList(new[] { firstArgument, secondArgument }));
							var generatedCodeAttribute = SyntaxFactory.Attribute(SyntaxFactory.ParseName("JsonDerivedType"), attributeArgumentList);
							var attributeList = SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(generatedCodeAttribute));
							declaration = declaration.AddAttributeLists(attributeList);
							counter ++;
						}
					}
					var compilationUnit = SyntaxFactory.CompilationUnit();
					foreach(var item in usings) {
						compilationUnit = compilationUnit.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(item)));
					}
					@namespace = @namespace.AddMembers(declaration);
					compilationUnit = compilationUnit.AddMembers(@namespace);
					var code = compilationUnit.NormalizeWhitespace().ToFullString();
					context.AddSource(found.Name, SourceText.From(code, Encoding.UTF8));
				}
			} else {
				string text = $"compilation is missing assembly name";
				var descriptor = new DiagnosticDescriptor("CmdInterfaceCodeGen02", "Command Interface CodeGen", "Missing assembly name", "Generator", DiagnosticSeverity.Warning, true, description: text);
				var warning = Diagnostic.Create(descriptor, Location.None);
				context.ReportDiagnostic(warning);
			}
		}

		public void Initialize(GeneratorInitializationContext context) {
		}
	}
}
