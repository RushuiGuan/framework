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

namespace Albatross.EFCore.CodeGen {
	[Generator]
	public class EntityModelBuilderClassCodeGen : ISourceGenerator {
		public void Execute(GeneratorExecutionContext context) {
			using var writer = new StringWriter();
			string? dbSessionNamespace = null;
			try {
				// System.Diagnostics.Debugger.Launch();
				var entityModelBuilderClasses = new List<INamedTypeSymbol>();

				foreach (var syntaxTree in context.Compilation.SyntaxTrees) {
					var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
					var walker = new EntityModelClassWalker(context.Compilation, semanticModel);
					walker.Visit(syntaxTree.GetRoot());
					entityModelBuilderClasses.AddRange(walker.EntityModelBuilderClasses);
					if (string.IsNullOrEmpty(dbSessionNamespace)) {
						dbSessionNamespace = walker.DbSessionClass?.ContainingNamespace.ToDisplayString();
					}
				}
				if (!entityModelBuilderClasses.Any()) {
					string text = $"No EntityModelBuilder class found.  Eligible classes should be public, have a default constructor and derived from the interface {My.EntityModelBuilderInterfaceName}";
					context.CodeGenDiagnostic(DiagnosticSeverity.Warning, $"{My.Diagnostic.IdPrefix}1", text);
				} else {
					// if the setup class is not found, use the namespace of the first option class
					if (string.IsNullOrEmpty(dbSessionNamespace)) {
						dbSessionNamespace = entityModelBuilderClasses.First().ContainingNamespace.ToDisplayString();
					}
				}
				var codeStack = new CodeStack();
				using (codeStack.NewScope(new CompilationUnitBuilder())) {
					codeStack.With(new UsingDirectiveNode("Albatross.EFCore"));
					codeStack.With(new UsingDirectiveNode("System.Collections.Generic"));
					using (codeStack.NewScope(new NamespaceDeclarationBuilder(dbSessionNamespace ?? "DbSessionNamespaceNotYetFound"))) {
						using (codeStack.NewScope(new ClassDeclarationBuilder("CodeGen").Static())) {
							using (codeStack.NewScope(new MethodDeclarationBuilder(new GenericIdentifierNode("List", "IBuildEntityModel"), "GatherBuilders").Static())) {
								codeStack.Begin(new VariableBuilder("list"))
									.Complete(new NewObjectBuilder(new GenericIdentifierNode("List", "IBuildEntityModel")))
									.End();

								foreach (var setup in entityModelBuilderClasses) {
									using (codeStack.NewScope()) {
										codeStack.With(new IdentifierNode("list"))
											.With(new IdentifierNode("Add"))
											.Begin(new ArgumentListBuilder())
												.Complete(new NewObjectBuilder(setup.GetFullName()))
											.End()
											.To(new InvocationExpressionBuilder());
									}
								}
								codeStack.With(SyntaxFactory.ReturnStatement(new IdentifierNode("list").Identifier));
							}
						}
					}
				}
				var code = codeStack.Build();
				context.AddSource("EntityModelBuilderExtensions", SourceText.From(code, Encoding.UTF8));
				writer.WriteLine("// EntityModelBuilderExtensions");
				writer.WriteLine(code);
			} catch (Exception err) {
				writer.WriteLine(err.ToString());
				context.CodeGenDiagnostic(DiagnosticSeverity.Error, $"{My.Diagnostic.IdPrefix}2", err.BuildCodeGeneneratorErrorMessage("commandline"));
			} finally {
				context.CreateGeneratorDebugFile("albatross-efcore-codegen.debug.txt", writer.ToString());
			}
		}
		public void Initialize(GeneratorInitializationContext context) { }
	}
}