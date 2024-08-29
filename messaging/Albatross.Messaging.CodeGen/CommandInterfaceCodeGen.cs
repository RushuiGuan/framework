using Albatross.CodeAnalysis.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.Messaging.CodeGen {
	/// <summary>
	/// The generator will first find any partial and empty interfaces that match the regex ^I[a-zA-Z0-9_]*Command$.
	/// It will then find any classes that implement these interfaces and generate a JsonDerivedType attribute for each class.
	/// </summary>
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
					var codeStack = new CodeStack()
						.Begin(new CompilationUnitBuilder())
							.With(new UsingDirectiveNode("System"), new UsingDirectiveNode("System.Text.Json.Serialization"))
							.Begin(new NamespaceDeclarationBuilder(candidate.ContainingNamespace.ToDisplayString()))
								.Begin(new InterfaceDeclarationBuilder(candidate.Name).Partial()).End()
							.End()
						.End();

					var attributeNames = new List<string>();
					var namespacesToImport = new List<string>();

					foreach (var syntaxTree in compilation.SyntaxTrees) {
						var semanticModel = compilation.GetSemanticModel(syntaxTree);
						var classWalker = new CommandInterfaceImplementationWalker(semanticModel, candidate.Name);
						classWalker.Visit(syntaxTree.GetRoot());
						codeStack.Seek(x => x is InterfaceDeclarationBuilder);
						foreach (var item in classWalker.Results) {
							if (!attributeNames.Contains(item.Name)) {
								namespacesToImport.Add(item.ContainingNamespace.ToDisplayString());
								attributeNames.Add(item.Name);
								codeStack.Begin(new AttributeBuilder("JsonDerivedType"))
									.Begin(new AttributeArgumentListBuilder())
										.With(SyntaxFactory.TypeOfExpression(SyntaxFactory.ParseTypeName(item.Name)))
										.With(new LiteralNode(item.Name))
									.End()
								.End();
							} else {
								string text = $"Interface {candidate.Name} has two class implementations of the same name {item.Name}";
								var descriptor = new DiagnosticDescriptor("CmdInterfaceCodeGen02", "Command Interface CodeGen", text, "Generator", DiagnosticSeverity.Error, true);
								context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
							}
						}
						codeStack.EndSeek();
					}
					codeStack.Seek(x => x is CompilationUnitBuilder)
						.With(namespacesToImport.Select(y => new UsingDirectiveNode(y)).ToArray())
						.EndSeek();
					var code = codeStack.Build();
					context.AddSource(candidate.Name, SourceText.From(code, Encoding.UTF8));
				}
			}
		}

		public void Initialize(GeneratorInitializationContext context) { }
	}
}
