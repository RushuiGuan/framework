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
				var setups = new List<CommandSetup>();

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
				} else {
					if (string.IsNullOrEmpty(setupClassNamespace)) {
						setupClassNamespace = optionClasses.First().ContainingNamespace.ToDisplayString();
					}
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
				foreach (var setup in setups) {
					var cs = new CodeStack();
					using (cs.NewScope(new CompilationUnitBuilder())) {
						cs.With(new UsingDirectiveNode("System.CommandLine"))
							.With(new UsingDirectiveNode("System"), new UsingDirectiveNode("System.IO"))
							.With(new UsingDirectiveNode("System.Threading.Tasks"));
						using (cs.NewScope(new NamespaceDeclarationBuilder(setup.OptionClass.ContainingNamespace.ToDisplayString()))) {
							using (cs.NewScope(new ClassDeclarationBuilder(setup.CommandClassName).Sealed().Partial())) {
								cs.With(new BaseTypeNode(My.CommandClassName));
								using (cs.NewScope(new ConstructorDeclarationBuilder(setup.CommandClassName))) {
									using (cs.NewScope(new ArgumentListBuilder())) {
										cs.With(new LiteralNode(setup.Name)).With(new LiteralNode(setup.Description));
									}
									this.BuildConstructorStatements(cs, setup, setup.OptionClass);
								}
								// build the option properties
								foreach (var option in setup.Options) {
									var optionType = new GenericIdentifierNode(My.OptionClassName, option.Type);
									cs.With(new PropertyNode(optionType, option.CommandOptionPropertyName).GetterOnly());
								}
							}
						}
					}
					var text = cs.Build();
					context.AddSource(setup.CommandClassName, SourceText.From(text, Encoding.UTF8));
					writer.WriteLine($"// {setup.CommandClassName}");
					writer.WriteLine(text);
				}

				var diCodeStack = new CodeStack();
				using (diCodeStack.NewScope(new CompilationUnitBuilder())) {
					diCodeStack.With(new UsingDirectiveNode("Microsoft.Extensions.DependencyInjection"));
					diCodeStack.With(new UsingDirectiveNode("System.CommandLine.Invocation"));
					diCodeStack.With(new UsingDirectiveNode("System.CommandLine.Hosting"));
					diCodeStack.With(new UsingDirectiveNode("Albatross.CommandLine"));

					var namespaces = new List<string>();
					var addedOptionClasses = new HashSet<string>();
					using (diCodeStack.NewScope(new NamespaceDeclarationBuilder(setupClassNamespace ?? "RootNamespaceNotYetFound"))) {
						using (diCodeStack.NewScope(new ClassDeclarationBuilder("RegistrationExtensions").Static())) {
							using (diCodeStack.NewScope(new MethodDeclarationBuilder("IServiceCollection", "RegisterCommands").Static())) {
								diCodeStack.With(new ParameterNode("IServiceCollection", "services").WithThis());
								foreach (var setup in setups) {
									using (diCodeStack.NewScope()) {
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
										using (diCodeStack.NewScope()) {
											diCodeStack.With(new IdentifierNode("services"))
												.With(new GenericIdentifierNode("AddOptions", setup.OptionClass.Name))
												.To(new MemberAccessBuilder())
												.To(new InvocationExpressionBuilder())
												.With(new IdentifierNode("BindCommandLine"))
												.To(new MemberAccessBuilder())
												.To(new InvocationExpressionBuilder());
										}
									}
								}
								diCodeStack.With(SyntaxFactory.ReturnStatement(new IdentifierNode("services").Identifier));
							}

							using (diCodeStack.NewScope(new MethodDeclarationBuilder("Setup", "AddCommands").Static())) {
								diCodeStack.With(new ParameterNode("Setup", "setup").WithThis());
								foreach (var setup in setups) {
									namespaces.Add(setup.OptionClass.ContainingNamespace.ToDisplayString());
									using (diCodeStack.NewScope()) {
										diCodeStack.With(new IdentifierNode("setup"))
											.With(new GenericIdentifierNode("AddCommand", setup.CommandClassName))
											.To(new InvocationExpressionBuilder());
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
				context.AddSource("RegistrationExtensions", SourceText.From(code, Encoding.UTF8));
				writer.WriteLine("// RegistrationExtensions");
				writer.WriteLine(code);
			} catch (Exception err) {
				writer.WriteLine(err.ToString());
				context.CodeGenDiagnostic(DiagnosticSeverity.Error, $"{My.Diagnostic.IdPrefix}2", err.BuildCodeGeneneratorErrorMessage("commandline"));
			} finally {
				context.CreateGeneratorDebugFile("albatross-commandline-codegen.debug.txt", writer.ToString());
			}
		}

		void BuildConstructorStatements(CodeStack cs, CommandSetup setup, INamedTypeSymbol optionClass) {
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
				// var variableName = "option";
				// cs.Complete(new VariableBuilder(My.OptionClassName, variableName));
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
						using (cs.NewScope()) {
							cs.With(new IdentifierNode(option.CommandOptionPropertyName))
								.With(new IdentifierNode("AddAlias"))
								.To(new MemberAccessBuilder())
								.ToNewBegin(new InvocationExpressionBuilder())
									.Begin(new ArgumentListBuilder())
										.With(new LiteralNode(alias.StartsWith("-") ? alias : $"-{alias}"))
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
		}


		public void Initialize(GeneratorInitializationContext context) { }
	}
}
