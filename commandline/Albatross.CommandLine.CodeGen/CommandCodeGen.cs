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

namespace Albatross.CommandLine.CodeGen {
	[Generator]
	public class CommandCodeGen : ISourceGenerator {
		public void Execute(GeneratorExecutionContext context) {
			// System.Diagnostics.Debugger.Launch();
			var optionClasses = new List<INamedTypeSymbol>();
			var handlerClasses = new List<INamedTypeSymbol>();

			foreach (var syntaxTree in context.Compilation.SyntaxTrees) {
				var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
				var walker = new CodeGenClassDeclarationWalker(semanticModel);
				walker.Visit(syntaxTree.GetRoot());
				optionClasses.AddRange(walker.CommandOptionClasses);
				handlerClasses.AddRange(walker.CommandHandlerClasses);
			}
			if (!optionClasses.Any()) {
				string text = $"No option class found.  Eligible classes should be public and annotated with the {My.VerbAttributeClass}";
				var descriptor = new DiagnosticDescriptor("CmdlineApiCodeGen01", "CommandLine CodeGen", text, "Generator", DiagnosticSeverity.Warning, true);
				context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
			}
			// ********* TODO: comment out when done
			using var writer = new StreamWriter(@"c:\temp\test.cs");
			// ********* TODO: comment out when done

			var qualifiedOptionClasses = new List<INamedTypeSymbol>();

			foreach (var optionClass in optionClasses) {
				if (optionClass.TryGetAttribute(My.VerbAttributeClass, out var verbAttribute)) {
					qualifiedOptionClasses.Add(optionClass);
					var className = GetCommandClassName(optionClass.Name);
					var cs = new CodeStack();

					using (cs.Begin(new CompilationUnitBuilder()).NewScope()) {
						cs.With(new UsingDirectiveNode("System.CommandLine"))
							.With(new UsingDirectiveNode("System"), new UsingDirectiveNode("System.IO"))
							.With(new UsingDirectiveNode("System.Threading.Tasks"));
						using (cs.Begin(new NamespaceDeclarationBuilder(optionClass.ContainingNamespace.ToDisplayString())).NewScope()) {
							using (cs.Begin(new ClassDeclarationBuilder(className).Partial()).NewScope()) {
								cs.With(new BaseTypeNode(My.CommandClassName));
								using (cs.Begin(new ConstructorDeclarationBuilder(className)).NewScope()) {
									using (cs.Begin(new ArgumentListBuilder()).NewScope()) {
										cs.With(new LiteralNode(GetCommandName(verbAttribute!)))
											.With(new LiteralNode(GetCommandDescription(verbAttribute!)));
									}
									this.BuildConstructorStatements(cs, verbAttribute!, optionClass);
								}
							}
						}
					}
					try {
						var code = cs.Build();
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

			var diCodeStack = new CodeStack();
			using (diCodeStack.Begin(new CompilationUnitBuilder()).NewScope()) {
				diCodeStack.With(new UsingDirectiveNode("Microsoft.Extensions.DependencyInjection"));
				diCodeStack.With(new UsingDirectiveNode("System.CommandLine.Hosting"));
				diCodeStack.With(new UsingDirectiveNode("Albatross.CommandLine"));
				var namespaces = new List<string>();
				using (diCodeStack.Begin(new NamespaceDeclarationBuilder("Albatross.CommandLine")).NewScope()) {
					using (diCodeStack.Begin(new ClassDeclarationBuilder("RegistrationExtensions").Static()).NewScope()) {
						using (diCodeStack.Begin(new MethodDeclarationBuilder("IServiceCollection", "RegisterCommands").Static()).NewScope()) {
							diCodeStack.With(new ParameterNode(true, "IServiceCollection", "services"));
							foreach (var optionClass in qualifiedOptionClasses) {
								namespaces.Add(optionClass.ContainingNamespace.ToDisplayString());
								var className = GetCommandClassName(optionClass.Name);
								using (diCodeStack.Begin().NewScope()) {
									diCodeStack.Complete(new InvocationExpressionBuilder(new IdentifierNode("services").WithGenericMember("AddOptions", optionClass.Name)))
										.With(new IdentifierNode("BindCommandLine")).Feed(new MemberAccessBuilder())
										.Feed(new InvocationExpressionBuilder());
								}
							}
							diCodeStack.With(SyntaxFactory.ReturnStatement(new IdentifierNode("services").Identifier));
						}

						using(diCodeStack.Begin(new MethodDeclarationBuilder("Setup", "AddCommandHandlers").Static()).NewScope()) {
							diCodeStack.With(new ParameterNode(true, "Setup", "setup"));
							foreach (var optionClass in qualifiedOptionClasses) {
								var className = GetCommandClassName(optionClass.Name);
								var handlerClass = handlerClasses.FirstOrDefault(x => x.Name == $"{className}Handler");
								if (handlerClass != null) {
									namespaces.Add(handlerClass.ContainingNamespace.ToDisplayString());
									diCodeStack.Complete(new InvocationExpressionBuilder(new IdentifierNode("setup").WithGenericMember("AddCommand", className, handlerClass.Name)));
								}
							}
							diCodeStack.With(SyntaxFactory.ReturnStatement(new IdentifierNode("setup").Identifier));
						}
					}
				}
				foreach (var item in namespaces) {
					diCodeStack.With(new UsingDirectiveNode(item));
				}
			}
			try {
				var code = diCodeStack.Build();
				context.AddSource("RegistrationExtensions", SourceText.From(code, Encoding.UTF8));
				// TODO: comment out when done
				writer.WriteLine("RegistrationExtensions");
				writer.WriteLine(code);
				// ********* TODO: comment out when done
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine(err.Message);
			}
		}

		string GetCommandName(AttributeData verbAttribute) {
			return verbAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? throw new ArgumentException("VerbAttribute is missing required argument");
		}
		string? GetCommandDescription(AttributeData verbAttribute) {
			return verbAttribute.ConstructorArguments.Skip(1).FirstOrDefault().Value?.ToString();
		}

		void BuildConstructorStatements(CodeStack cs, AttributeData verbAttribute, INamedTypeSymbol optionClass) {
			if (verbAttribute.TryGetNamedArgument("Alias", out var verbAlias)) {
				foreach (var value in verbAlias.Values) {
					using (cs.Begin(new InvocationExpressionBuilder(new IdentifierNode().WithMember("AddAlias"))).NewScope()) {
						using (cs.Begin(new ArgumentListBuilder()).NewScope()) {
							cs.With(new LiteralNode($"{value.Value}"));
						}
					}
				}
			}
			var propertySymbols = optionClass.GetMembers().OfType<IPropertySymbol>()
				.Where(s => s.SetMethod?.DeclaredAccessibility == Accessibility.Public
					&& s.GetMethod?.DeclaredAccessibility == Accessibility.Public)
				.ToList();

			if (propertySymbols.Any()) {
				var variableName = "option";
				cs.Begin(new VariableBuilder(My.OptionClassName, variableName)).End();
				foreach (var propertySymbol in propertySymbols) {
					var name = $"--{propertySymbol.Name.Kebaberize()}";
					var type = propertySymbol.Type.ToDisplayString();
					string description = null;
					List<string> aliases = new List<string>();
					bool? isHidden = null;
					var required = !propertySymbol.IsNullable();

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
						if (optionAttribute!.TryGetNamedArgument("Required", out var requiredConstant)) {
							required = Convert.ToBoolean(requiredConstant.Value);
						}
					}

					using (cs.Begin(new AssignmentExpressionBuilder(variableName)).NewScope()) {
						using (cs.Begin(new NewObjectBuilder(My.OptionClassName, type)).NewScope()) {
							using (cs.Begin(new ArgumentListBuilder()).NewScope()) {
								cs.With(new LiteralNode(name)).With(new LiteralNode(description));
							}
							using (cs.Begin(new AssignmentExpressionBuilder("IsRequired")).NewScope()) {
								cs.With(new LiteralNode(required));
							}
							if (isHidden.HasValue) {
								cs.Begin(new AssignmentExpressionBuilder("IsHidden"))
									.With(new LiteralNode(isHidden.Value))
								.End();
							}
						}
					}

					foreach (var alias in aliases) {
						using (cs.Begin(new InvocationExpressionBuilder(new IdentifierNode(variableName).WithMember("AddAlias"))).NewScope()) {
							using (cs.Begin(new ArgumentListBuilder()).NewScope()) {
								cs.With(new LiteralNode($"-{alias}"));
							}
						}
					}
					using (cs.Begin(new InvocationExpressionBuilder(new IdentifierNode().WithMember("AddOption"))).NewScope()) {
						using (cs.Begin(new ArgumentListBuilder()).NewScope()) {
							cs.With(SyntaxFactory.IdentifierName(variableName));
						}
					}
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
