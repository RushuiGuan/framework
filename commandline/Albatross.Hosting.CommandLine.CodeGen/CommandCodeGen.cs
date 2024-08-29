using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeAnalysis.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;

namespace Albatross.Hosting.CommandLine.CodeGen {
	[Generator]
	public class CommandCodeGen : ISourceGenerator {
		public void Execute(GeneratorExecutionContext context) {
			// System.Diagnostics.Debugger.Launch();
			List<INamedTypeSymbol> optionClasses = new List<INamedTypeSymbol>();

			foreach (var syntaxTree in context.Compilation.SyntaxTrees) {
				var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
				var walker = new CommandOptionsDeclarationWalker(semanticModel);
				walker.Visit(syntaxTree.GetRoot());
				optionClasses.AddRange(walker.Result);
			}
			if (!optionClasses.Any()) {
				string text = $"No option class found.  Eligible classes should be public and annotated with the {My.VerbAttributeClass}";
				var descriptor = new DiagnosticDescriptor("CmdlineApiCodeGen01", "CommandLine CodeGen", text, "Generator", DiagnosticSeverity.Warning, true);
				context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
			}
			// ********* TODO: comment out when done
			using var writer = new StreamWriter(@"c:\temp\test.cs");
			// ********* TODO: comment out when done
			foreach (var optionClass in optionClasses) {
				if (optionClass.TryGetAttribute(My.VerbAttributeClass, out var verbAttribute)) {
					var className = GetCommandClassName(optionClass.Name);
					var codeStack = new CodeStack().Begin(new CompilationUnitBuilder())
							.With(new UsingDirectiveNode(My.SysCommandLineNamespace))
							.Begin(new NamespaceDeclarationBuilder(optionClass.ContainingNamespace.ToDisplayString()))
								.Begin(new ClassDeclarationBuilder(className).Partial())
									.With(new BaseTypeNode(My.CommandClassName))
									.Begin(new ConstructorDeclarationBuilder(className))
										.Begin(new ArgumentListBuilder())
											.With(new LiteralNode(GetCommandName(verbAttribute!)))
											.With(new LiteralNode(GetCommandDescription(verbAttribute!)))
										.End()
										.Append(cs => this.BuildConstructorStatements(cs, verbAttribute!, optionClass))
									.End()
								.End()
							.End()
						.End();
					try {
						var code = codeStack.Build();
						context.AddSource(className, SourceText.From(code, Encoding.UTF8));
						// TODO: comment out when done
						writer.WriteLine(className);
						writer.WriteLine(code);
						// ********* TODO: comment out when done
					} catch (Exception err) {
						System.Diagnostics.Debug.WriteLine(err.Message);
					}
				}
			}
		}

		string GetCommandName(AttributeData verbAttribute) {
			return verbAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? throw new ArgumentException("VerbAttribute is missing required argument");
		}
		string? GetCommandDescription(AttributeData verbAttribute) {
			return verbAttribute.ConstructorArguments.Skip(1).FirstOrDefault().Value?.ToString();
		}

		void BuildConstructorStatements(CodeStack codeStack, AttributeData verbAttribute, INamedTypeSymbol optionClass) {
			if (verbAttribute.TryGetNamedArgument("Alias", out var verbAlias)) {
				foreach (var value in verbAlias.Values) {
					codeStack.Begin(new InvocationExpressionBuilder(true, "AddAlias"))
						.Begin(new ArgumentListBuilder())
							.With(new LiteralNode($"--{value.Value}"))
						.End()
					.End(true);
				}
			}

			var variableName = "option";
			codeStack.Begin(new VariableBuilder(My.OptionClassName, variableName)).End(true);

			foreach (var propertySymbol in optionClass.GetMembers().OfType<IPropertySymbol>()) {
				if (propertySymbol.TryGetAttribute(My.OptionAttributeClass, out var optionAttribute)) {
					var name = $"--{optionAttribute!.ConstructorArguments.FirstOrDefault().Value}";
					var description = optionAttribute.ConstructorArguments.Skip(1).FirstOrDefault().Value?.ToString();
					var type = propertySymbol.Type.ToDisplayString();
					codeStack.Begin(new AssignmentExpressionBuilder(variableName))
							.Begin(new NewObjectBuilder(My.OptionClassName, type))
								.Begin(new ArgumentListBuilder())
									.With(new LiteralNode(name))
									.With(new LiteralNode(description))
								.End()
							.End()
						.End(true);
					if (optionAttribute.TryGetNamedArgument("Alias", out var optionAlias)) {
						foreach (var value in optionAlias.Values) {
							codeStack.Begin(new InvocationExpressionBuilder(variableName, "AddAlias"))
									.Begin(new ArgumentListBuilder())
										.With(new LiteralNode($"--{value.Value}"))
									.End()
								.End(true);
						}
					}
					if (optionAttribute.TryGetNamedArgument("Required", out var required)) {
						codeStack.Begin(new AssignmentExpressionBuilder(variableName, "IsRequired"))
								.With(new LiteralNode(Convert.ToBoolean(required.Value)))
							.End(true);
					}
					if (optionAttribute.TryGetNamedArgument("Hidden", out var hidden)) {
						codeStack.Begin(new InvocationExpressionBuilder(variableName, "IsHidden"))
							.With(new LiteralNode(Convert.ToBoolean(hidden.Value)))
							.End(true);
					}
					codeStack.Begin(new InvocationExpressionBuilder(true, "AddOption"))
						.Begin(new ArgumentListBuilder())
							.With(SyntaxFactory.IdentifierName(variableName))
						.End()
					.End(true);
				}
			}
		}

		string GetCommandClassName(string optionsClassName) {
			if (optionsClassName.EndsWith(My.Postfix_Options, StringComparison.InvariantCultureIgnoreCase)) {
				return optionsClassName.Substring(0, optionsClassName.Length - My.Postfix_Options.Length);
			} else {
				return optionsClassName;
			}
		}
		public void Initialize(GeneratorInitializationContext context) { }
	}
}
