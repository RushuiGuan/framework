using Shared = Albatross.CodeAnalysis.My;
using Albatross.CodeAnalysis;
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
			using var writer = new StringWriter();
			string? setupClassNamespace = null;
			try {
				// System.Diagnostics.Debugger.Launch();
				var optionClasses = new List<INamedTypeSymbol>();
				var handlerClasses = new Dictionary<string, string>();
				var setups = new SortedDictionary<string, CommandSetup>();

				foreach (var syntaxTree in context.Compilation.SyntaxTrees) {
					var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
					var walker = new CodeGenClassDeclarationWalker(context.Compilation, semanticModel);
					walker.Visit(syntaxTree.GetRoot());
					optionClasses.AddRange(walker.CommandOptionClasses);
					if (string.IsNullOrEmpty(setupClassNamespace)) {
						setupClassNamespace = walker.SetupClass?.ContainingNamespace.ToDisplayString();
					}
				}
				if (!optionClasses.Any()) {
					string text = $"No option class found.  Eligible classes should be public and annotated with the {My.VerbAttributeClass}";
					context.CodeGenDiagnostic(DiagnosticSeverity.Warning, $"{My.Diagnostic.IdPrefix}1", text);
					return;
				}
				// if the setup class is not found, use the namespace of the first option class
				if (string.IsNullOrEmpty(setupClassNamespace)) {
					setupClassNamespace = optionClasses.First().ContainingNamespace.ToDisplayString();
				}
				foreach (var optionClass in optionClasses) {
					foreach (var attribute in optionClass.GetAttributes()) {
						if (attribute.AttributeClass?.GetFullName() == My.VerbAttributeClass) {
							var setup = new CommandSetup(optionClass, attribute);
							if (setups.ContainsKey(setup.Key)) {
								context.CodeGenDiagnostic(DiagnosticSeverity.Error, $"{My.Diagnostic.IdPrefix}3", $"Duplicate command key '{setup.Key}' found");
							} else {
								setups.Add(setup.Key, setup);
							}
						}
					}
				}
				foreach (var group in setups.Values.GroupBy(x => x.CommandClassName)) {
					if (group.Count() > 1) {
						int index = 0;
						foreach (var setup in group) {
							setup.RenameCommandClass(index++);
						}
					}
				}
				// generate the command class
				foreach (var setup in setups.Values) {
					var cs = new CodeStack();
					using (cs.NewScope(new CompilationUnitBuilder())) {
						cs.With(new UsingDirectiveNode("System.CommandLine"))
							.With(new UsingDirectiveNode(Shared.Namespace.System))
							.With(new UsingDirectiveNode(Shared.Namespace.System_IO))
							.With(new UsingDirectiveNode(Shared.Namespace.System_Threading_Tasks));
						IEnumerable<string> additionalNamespaces = [];
						using (cs.NewScope(new NamespaceDeclarationBuilder(setup.OptionClass.ContainingNamespace.ToDisplayString()))) {
							using (cs.NewScope(new ClassDeclarationBuilder(setup.CommandClassName).Sealed().Partial())) {
								cs.With(new BaseTypeNode(My.CommandClassName));
								using (cs.NewScope(new ConstructorDeclarationBuilder(setup.CommandClassName))) {
									using (cs.NewScope(new ArgumentListBuilder())) {
										cs.With(new LiteralNode(setup.Name)).With(new LiteralNode(setup.Description));
									}
									additionalNamespaces = this.BuildConstructorStatements(cs, setup);
								}
								// build the option properties
								foreach (var option in setup.Options) {
									var optionType = new GenericIdentifierNode(My.OptionClassName, option.Type);
									cs.With(new PropertyNode(optionType, option.CommandOptionPropertyName).GetterOnly());
								}
							}
						}
						foreach (var item in additionalNamespaces) {
							cs.With(new UsingDirectiveNode(item));
						}
					}
					var text = cs.Build();
					context.AddSource(setup.CommandClassName, SourceText.From(text, Encoding.UTF8));
					writer.WriteSourceHeader(setup.CommandClassName);
					writer.WriteLine(text);
				}
				// generate the code to register the commands
				var diCodeStack = new CodeStack();
				using (diCodeStack.NewScope(new CompilationUnitBuilder())) {
					diCodeStack.With(new UsingDirectiveNode(Shared.Namespace.Microsoft_Extensions_DependencyInjection));
					diCodeStack.With(new UsingDirectiveNode("System.CommandLine.Invocation"));
					diCodeStack.With(new UsingDirectiveNode("System.CommandLine.Hosting"));
					diCodeStack.With(new UsingDirectiveNode("System.CommandLine"));
					diCodeStack.With(new UsingDirectiveNode("Albatross.CommandLine"));
					diCodeStack.With(new UsingDirectiveNode("System.Collections.Generic"));

					var namespaces = new List<string>();
					var addedOptionClasses = new HashSet<string>();
					using (diCodeStack.NewScope(new NamespaceDeclarationBuilder(setupClassNamespace ?? "RootNamespaceNotYetFound"))) {
						using (diCodeStack.NewScope(new ClassDeclarationBuilder(Shared.Class.CodeGenExtensions).Static())) {
							using (diCodeStack.NewScope(new MethodDeclarationBuilder("IServiceCollection", "RegisterCommands").Static())) {
								diCodeStack.With(new ParameterNode("IServiceCollection", "services").WithThis());
								foreach (var setup in setups.Values) {
									using (diCodeStack.NewScope()) {
										diCodeStack.With(new IdentifierNode("services"))
											.With(new GenericIdentifierNode("AddKeyedScoped", "ICommandHandler", setup.HandlerClass))
											.To(new MemberAccessBuilder())
											.Begin(new ArgumentListBuilder()).With(new LiteralNode(setup.Key)).End()
											.To(new InvocationExpressionBuilder());
									}
									if (!addedOptionClasses.Contains(setup.OptionClass.Name)) {
										addedOptionClasses.Add(setup.OptionClass.Name);
										namespaces.Add(setup.OptionClass.ContainingNamespace.ToDisplayString());
										var className = setup.CommandClassName;
										using (diCodeStack.NewScope()) {
											diCodeStack.With(new IdentifierNode("services"))
												.With(new GenericIdentifierNode("AddOptions", setup.OptionClass.Name))
												.To(new InvocationExpressionBuilder())
												.With(new IdentifierNode("BindCommandLine"))
												.To(new InvocationExpressionBuilder());
										}
									}
								}
								diCodeStack.With(SyntaxFactory.ReturnStatement(new IdentifierNode("services").Identifier));
							}

							using (diCodeStack.NewScope(new MethodDeclarationBuilder("Setup", "AddCommands").Static())) {
								diCodeStack.With(new ParameterNode("Setup", "setup").WithThis());

								diCodeStack.Begin(new VariableBuilder("dict"))
									.Complete(new NewObjectBuilder(new GenericIdentifierNode("Dictionary", "string", "Command")))
								.End();

								using (diCodeStack.NewScope()) {
									diCodeStack.With(new IdentifierNode("dict"))
										.With(new IdentifierNode("Add"))
										.ToNewBegin(new InvocationExpressionBuilder())
											.Begin(new ArgumentListBuilder())
												.Begin(new MemberAccessBuilder())
													.With(new IdentifierNode("string"))
													.With(new IdentifierNode("Empty"))
												.End()
												.Begin(new MemberAccessBuilder())
													.With(new IdentifierNode("setup"))
													.With(new IdentifierNode("RootCommand"))
												.End()
											.End()
										.End();
								}

								foreach (var setup in setups.Values) {
									namespaces.Add(setup.OptionClass.ContainingNamespace.ToDisplayString());
									using (diCodeStack.NewScope()) {
										diCodeStack.With(new IdentifierNode("dict"))
											.With(new GenericIdentifierNode("AddCommand", setup.CommandClassName))
											.ToNewBegin(new InvocationExpressionBuilder())
												.Begin(new ArgumentListBuilder())
													.With(new LiteralNode(setup.Key))
													.With(new IdentifierNode("setup"))
												.End()
											.End();
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
				var code = diCodeStack.Build();
				context.AddSource(Shared.Class.CodeGenExtensions, SourceText.From(code, Encoding.UTF8));
				writer.WriteSourceHeader(Shared.Class.CodeGenExtensions);
				writer.WriteLine(code);
			} catch (Exception err) {
				writer.WriteLine(err.ToString());
				context.CodeGenDiagnostic(DiagnosticSeverity.Error, $"{My.Diagnostic.IdPrefix}2", err.BuildCodeGeneneratorErrorMessage("commandline"));
			} finally {
				var text = writer.ToString();
				if (!string.IsNullOrEmpty(text)) {
					context.CreateGeneratorDebugFile("albatross-commandline-codegen.debug.txt", text);
				}
			}
		}

		IEnumerable<string> BuildConstructorStatements(CodeStack cs, CommandSetup setup) {
			var namespaces = new HashSet<string>();
			foreach (var value in setup.Aliases) {
				using (cs.NewScope()) {
					cs.With(new ThisExpression()).With(new IdentifierNode("AddAlias"))
						.To(new MemberAccessBuilder())
						.ToNewBegin(new InvocationExpressionBuilder())
							.Begin(new ArgumentListBuilder())
								.With(new LiteralNode($"{value}"))
							.End()
						.End();
				}
			}
			if (setup.Options.Any()) {
				foreach (var option in setup.Options) {
					using (cs.NewScope()) {
						using (cs.With(new ThisExpression()).With(new IdentifierNode(option.CommandOptionPropertyName)).ToNewScope(new AssignmentExpressionBuilder())) {
							using (cs.NewScope(new NewObjectBuilder(new GenericIdentifierNode(My.OptionClassName, option.Type)))) {
								using (cs.NewScope(new ArgumentListBuilder())) {
									cs.With(new LiteralNode(option.Name)).With(new LiteralNode(option.Description));
								}
								if (option.Required) {
									using (cs.NewScope(new AssignmentExpressionBuilder("IsRequired"))) {
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
					}

					foreach (var alias in option.Aliases) {
						string aliasName;
						if (alias.StartsWith("-")) {
							aliasName = alias;
						} else {
							aliasName = $"-{alias}";
						}
						using (cs.NewScope()) {
							cs.With(new IdentifierNode(option.CommandOptionPropertyName))
								.With(new IdentifierNode("AddAlias"))
								.To(new MemberAccessBuilder())
								.ToNewBegin(new InvocationExpressionBuilder())
									.Begin(new ArgumentListBuilder())
										.With(new LiteralNode(aliasName))
									.End()
								.End();
						}
					}
					if (option.ShouldDefaultToInitializer && option.PropertyInitializer != null) {
						using (cs.NewScope()) {
							namespaces.Add(option.Property.Type.ContainingNamespace.ToDisplayString());
							cs.With(new IdentifierNode(option.CommandOptionPropertyName))
								.With(new IdentifierNode("SetDefaultValue"))
								.ToNewBegin(new InvocationExpressionBuilder())
									.Begin(new ArgumentListBuilder())
										.With(new NodeContainer(option.PropertyInitializer))
									.End()
								.End();
						}
					}
					using (cs.NewScope()) {
						cs.With(new ThisExpression())
							.With(new IdentifierNode("AddOption"))
							.To(new MemberAccessBuilder())
							.ToNewBegin(new InvocationExpressionBuilder())
								.Begin(new ArgumentListBuilder())
									.With(new IdentifierNode(option.CommandOptionPropertyName))
								.End()
							.End();
					}
				}
			}
			return namespaces;
		}

		public void Initialize(GeneratorInitializationContext context) { }
	}
}