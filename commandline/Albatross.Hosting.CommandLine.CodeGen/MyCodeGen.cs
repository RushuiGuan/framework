using Albatross.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.Hosting.CommandLine.CodeGen {
	[Generator]
	public class MyCodeGen : ISourceGenerator {
		public void Execute(GeneratorExecutionContext context) {
			System.Diagnostics.Debugger.Launch();
			var compilation = context.Compilation;

			List<INamedTypeSymbol> optionsClassSymbols = new List<INamedTypeSymbol>();

			foreach (var syntaxTree in compilation.SyntaxTrees) {
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var walker = new CommandOptionsDeclarationWalker(semanticModel);
				walker.Visit(syntaxTree.GetRoot());
				optionsClassSymbols.AddRange(walker.Result);
			}
			if (!optionsClassSymbols.Any()) {
				string text = $"No option class found.  Eligible classess are public and annotated with the {My.VerbAttributeClass}";
				var descriptor = new DiagnosticDescriptor("CmdlineApiCodeGen01", "CommandLine CodeGen", text, "Generator", DiagnosticSeverity.Warning, true);
				context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
			}
			foreach (var candidate in optionsClassSymbols) {
				if (candidate.TryGetAttribute(My.VerbAttributeClass, out var verbAttribute)) {
					var className = GetCommandClassName(candidate.Name);
					var declaration = SyntaxFactory.ClassDeclaration(className)
						.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
						.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))
						.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(My.CommandClassName)));

					var constructor = this.BuildConstructor(candidate, className, verbAttribute!);
					declaration = declaration.AddMembers(constructor);

					var compilationUnit = SyntaxFactory.CompilationUnit();
					compilationUnit = compilationUnit.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(My.SysCommandLineNamespace)));
					var @namespace = candidate.ContainingNamespace.ToDisplayString().NamespaceDeclaration();
					@namespace = @namespace.AddMembers(declaration);
					compilationUnit = compilationUnit.AddMembers(@namespace);
					var code = compilationUnit.NormalizeWhitespace().ToFullString();
					context.AddSource(className, SourceText.From(code, Encoding.UTF8));
				}
			}
		}
		StatementSyntax BuildCreateOptionStatement(IPropertySymbol propertySymbol, AttributeData optionAttribute) {
			var name = optionAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
			var description = optionAttribute.ConstructorArguments.Skip(1).FirstOrDefault().Value?.ToString();
			// var alias = optionAttribute.NamedArguments.Where(x => x.Key == "Alias").Select(x => x.Value.Value).FirstOrDefault();
			var type = propertySymbol.Type.ToDisplayString();
			var option = SyntaxFactory.ParseExpression($"new Option<{type}>(\"{name}\", \"{description}\")");
			//if (alias != null) {
			//	option = SyntaxFactory.ParseExpression($"new Option<{type}({{ \"{name}\", \"{description}\" }}).Alias(\"{alias}\")");
			//}
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.ParseExpression($"this.AddOption({option})"));
		}
		ConstructorInitializerSyntax AddConstructorBaseTypeInitizliation(AttributeData verbAttribute) {
			var name = verbAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? string.Empty;
			var description = verbAttribute.ConstructorArguments.Skip(1).FirstOrDefault().Value?.ToString();
			var builder = new ArgumentListBuilder()
				.Add(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(name)));
			if (description == null) {
				builder.Add(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));
			} else {
				builder.Add(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(description)));
			}
			return SyntaxFactory.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer, builder.Build());
		}
		ConstructorDeclarationSyntax BuildConstructor(INamedTypeSymbol optionClass, string className, AttributeData verbAttribute) {
			var name = verbAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
			var description = verbAttribute.ConstructorArguments.Skip(1).FirstOrDefault().Value?.ToString();
			var constructor = SyntaxFactory.ConstructorDeclaration(className)
				.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
				.WithInitializer(this.AddConstructorBaseTypeInitizliation(verbAttribute));

			var statements = new List<StatementSyntax>();
			foreach (var propertySymbol in optionClass.GetMembers().OfType<IPropertySymbol>()) {
				if (propertySymbol.TryGetAttribute(My.OptionAttributeClass, out var optionAttribute)) {
					statements.Add(this.BuildCreateOptionStatement(propertySymbol, optionAttribute!));
				}
			}
			constructor = constructor.WithBody(SyntaxFactory.Block(statements));
			return constructor;
			//var alias = verbAttribute.NamedArguments.Where(x => x.Key == "Alias").Select(x => x.Value.Value).FirstOrDefault();
		}
		string GetCommandClassName(string optionsClassName) {
			if (optionsClassName.EndsWith("Options")) {
				return optionsClassName.Substring(0, optionsClassName.Length - "Options".Length);
			} else {
				return optionsClassName;
			}
		}
		public void Initialize(GeneratorInitializationContext context) { }
	}
}
