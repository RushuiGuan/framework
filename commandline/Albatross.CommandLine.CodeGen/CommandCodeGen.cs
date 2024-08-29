using Humanizer;
using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeAnalysis.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
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
							.With(new UsingDirectiveNode("System.CommandLine"))
							.With(new UsingDirectiveNode("System"), new UsingDirectiveNode("System.IO"))
							.With(new UsingDirectiveNode("System.Threading.Tasks"))
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
							.With(new LiteralNode($"{value.Value}"))
						.End()
					.End(true);
				}
			}
			var propertySymbols = optionClass.GetMembers().OfType<IPropertySymbol>()
				.Where(s=>s.SetMethod?.DeclaredAccessibility == Accessibility.Public 
					&& s.GetMethod?.DeclaredAccessibility == Accessibility.Public)
				.ToList();
			if (propertySymbols.Any()) {
				var variableName = "option";
				codeStack.Begin(new VariableBuilder(My.OptionClassName, variableName)).End(true);
				foreach (var propertySymbol in propertySymbols) {
					var name = $"--{propertySymbol.Name.Kebaberize()}";
					var type = propertySymbol.Type.ToDisplayString();
					string description = null;
					List<string> aliases = new List<string>();
					bool? isHidden = null;

					if (propertySymbol.TryGetAttribute(My.OptionAttributeClass, out var optionAttribute)) {
						if (optionAttribute!.TryGetNamedArgument("Description", out var descriptionConstant)) {
							description = descriptionConstant.Value?.ToString();
						}
						if (optionAttribute!.TryGetNamedArgument("Alias", out var optionAliasConstant)) {
							foreach (var value in optionAliasConstant.Values) {
								if (value.Value != null) {
									aliases.Add(value.Value.ToString());
								}
							}
						}
						if (optionAttribute!.TryGetNamedArgument("Hidden", out var hiddenConstant)) {
							isHidden = Convert.ToBoolean(hiddenConstant.Value);
						}
					}

					codeStack.Begin(new AssignmentExpressionBuilder(variableName))
						.Begin(new NewObjectBuilder(My.OptionClassName, type))
							.Begin(new ArgumentListBuilder())
								.With(new LiteralNode(name))
								.With(new LiteralNode(description))
							.End()
							.Begin(new AssignmentExpressionBuilder("IsRequired"))
								.With(new LiteralNode(!propertySymbol.IsNullable()))
							.End()
							.Append(cs => {
								if (isHidden.HasValue) {
									cs.Begin(new AssignmentExpressionBuilder("IsHidden"))
										.With(new LiteralNode(isHidden.Value))
									.End();
								}
							})
						.End()
					.End(true);

					foreach (var alias in aliases) {
						codeStack.Begin(new InvocationExpressionBuilder(variableName, "AddAlias"))
							.Begin(new ArgumentListBuilder())
								.With(new LiteralNode($"-{alias}"))
							.End()
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
