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
			var handlerClasses = new Dictionary<string, string>();
			var setups = new List<CommandSetup>();

			foreach (var syntaxTree in context.Compilation.SyntaxTrees) {
				var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
				var walker = new CodeGenClassDeclarationWalker(semanticModel);
				walker.Visit(syntaxTree.GetRoot());
				optionClasses.AddRange(walker.CommandOptionClasses);
			}
			if (!optionClasses.Any()) {
				string text = $"No option class found.  Eligible classes should be public and annotated with the {My.VerbAttributeClass}";
				var descriptor = new DiagnosticDescriptor("CmdlineApiCodeGen01", "CommandLine CodeGen", text, "Generator", DiagnosticSeverity.Warning, true);
				context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
			} else {
				foreach (var optionClass in optionClasses) {
					foreach (var attribute in optionClass.GetAttributes()) {
						if (attribute.AttributeClass?.GetFullName() == My.VerbAttributeClass) {
							var setup = new CommandSetup(optionClass, attribute);
							setups.Add(setup);
						}
					}
				}
				foreach (var group in setups.GroupBy(x => x.CommandClassName)) {
					if (group.Count() > 1) {
						int index = 0;
						foreach (var setup in group) {
							setup.RenameCommandClass(index++);
						}
					}
				}
			}
			// ********* TODO: comment out when done
			using var writer = new StreamWriter(@"c:\temp\test.cs");
			// ********* TODO: comment out when done
			foreach (var setup in setups) {
				var cs = new CodeStack();
				using (cs.Begin(new CompilationUnitBuilder()).NewScope()) {
					cs.With(new UsingDirectiveNode("System.CommandLine"))
						.With(new UsingDirectiveNode("System"), new UsingDirectiveNode("System.IO"))
						.With(new UsingDirectiveNode("System.Threading.Tasks"));
					using (cs.Begin(new NamespaceDeclarationBuilder(setup.OptionClass.ContainingNamespace.ToDisplayString())).NewScope()) {
						using (cs.Begin(new ClassDeclarationBuilder(setup.CommandClassName).Partial()).NewScope()) {
							cs.With(new BaseTypeNode(My.CommandClassName));
							using (cs.Begin(new ConstructorDeclarationBuilder(setup.CommandClassName)).NewScope()) {
								using (cs.Begin(new ArgumentListBuilder()).NewScope()) {
									cs.With(new LiteralNode(setup.Name))
										.With(new LiteralNode(setup.Description));
								}
								this.BuildConstructorStatements(cs, setup, setup.OptionClass);
							}
						}
					}
				}
				try {
					var code = cs.Build();
					context.AddSource(setup.CommandClassName, SourceText.From(code, Encoding.UTF8));
					// TODO: comment out when done
					 writer.WriteLine(setup.CommandClassName);
					 writer.WriteLine(code);
					// ********* TODO: comment out when done
				} catch (Exception err) {
					System.Diagnostics.Debug.WriteLine(err.Message);
				}
			}

			var diCodeStack = new CodeStack();
			using (diCodeStack.Begin(new CompilationUnitBuilder()).NewScope()) {
				diCodeStack.With(new UsingDirectiveNode("Microsoft.Extensions.DependencyInjection"));
				diCodeStack.With(new UsingDirectiveNode("System.CommandLine.Invocation"));
				diCodeStack.With(new UsingDirectiveNode("System.CommandLine.Hosting"));
				diCodeStack.With(new UsingDirectiveNode("Albatross.CommandLine"));
				var namespaces = new List<string>();
				var addedOptionClasses = new HashSet<string>();
				using (diCodeStack.Begin(new NamespaceDeclarationBuilder("Albatross.CommandLine")).NewScope()) {
					using (diCodeStack.Begin(new ClassDeclarationBuilder("RegistrationExtensions").Static()).NewScope()) {
						using (diCodeStack.Begin(new MethodDeclarationBuilder("IServiceCollection", "RegisterCommands").Static()).NewScope()) {
							diCodeStack.With(new ParameterNode(true, "IServiceCollection", "services"));
							foreach (var setup in setups) {
								using (diCodeStack.Begin().NewScope()) {
									diCodeStack.With(new IdentifierNode("services"))
										.With(new GenericIdentifierNode("AddKeyedScoped", "ICommandHandler", setup.HandlerClass))
										.To(new MemberAccessBuilder())
										.Begin(new ArgumentListBuilder()).With(new LiteralNode(setup.Name)).End()
										.To(new InvocationExpressionBuilder());
								}
								if (!addedOptionClasses.Contains(setup.OptionClass.Name)) {
									addedOptionClasses.Add(setup.OptionClass.Name);
									namespaces.Add(setup.OptionClass.ContainingNamespace.ToDisplayString());
									var className = setup.CommandClassName;
									using (diCodeStack.Begin().NewScope()) {
										diCodeStack.Complete(new InvocationExpressionBuilder(new IdentifierNode("services").WithGenericMember("AddOptions", setup.OptionClass.Name)))
											.With(new IdentifierNode("BindCommandLine"))
											.To(new MemberAccessBuilder())
											.To(new InvocationExpressionBuilder());
									}
								}
							}
							diCodeStack.With(SyntaxFactory.ReturnStatement(new IdentifierNode("services").Identifier));
						}

						using (diCodeStack.Begin(new MethodDeclarationBuilder("Setup", "AddCommands").Static()).NewScope()) {
							diCodeStack.With(new ParameterNode(true, "Setup", "setup"));
							foreach (var setup in setups) {
								namespaces.Add(setup.OptionClass.ContainingNamespace.ToDisplayString());
								diCodeStack.Complete(new InvocationExpressionBuilder(new IdentifierNode("setup").WithGenericMember("AddCommand", setup.CommandClassName)));
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

		void BuildConstructorStatements(CodeStack cs, CommandSetup setup, INamedTypeSymbol optionClass) {
			foreach (var value in setup.Aliases) {
				using (cs.Begin(new InvocationExpressionBuilder(new IdentifierNode().WithMember("AddAlias"))).NewScope()) {
					using (cs.Begin(new ArgumentListBuilder()).NewScope()) {
						cs.With(new LiteralNode($"{value}"));
					}
				}
			}
			if(setup.Options.Any()) {
				var variableName = "option";
				cs.Complete(new VariableBuilder(My.OptionClassName, variableName));
				foreach (var option in setup.Options) {
					using (cs.Begin(new AssignmentExpressionBuilder(variableName)).NewScope()) {
						using (cs.Begin(new NewObjectBuilder(My.OptionClassName, option.Type)).NewScope()) {
							using (cs.Begin(new ArgumentListBuilder()).NewScope()) {
								cs.With(new LiteralNode(option.Name)).With(new LiteralNode(option.Description));
							}
							if (option.Required) {
								using (cs.Begin(new AssignmentExpressionBuilder("IsRequired")).NewScope()) {
									cs.With(new LiteralNode(true));
								}
							}
							if (option.Hidden) {
								cs.Begin(new AssignmentExpressionBuilder("IsHidden"))
									.With(new LiteralNode(true))
								.End();
							}
						}
					}

					foreach (var alias in option.Aliases) {
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


		public void Initialize(GeneratorInitializationContext context) { }
	}
}
