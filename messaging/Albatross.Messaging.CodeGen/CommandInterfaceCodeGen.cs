using Shared = Albatross.CodeAnalysis.My;
using Albatross.CodeAnalysis;
using Albatross.CodeAnalysis.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Albatross.CodeAnalysis.Symbols;

namespace Albatross.Messaging.CodeGen {
	/// <summary>
	/// The generator will first find any partial and empty interfaces that match the regex ^I[a-zA-Z0-9_]*Command$.
	/// It will then find any classes that implement these interfaces and generate a JsonDerivedType attribute for each class.
	/// </summary>
	[Generator]
	public class CommandInterfaceCodeGen : ISourceGenerator {
		public static void Combine<K, R>(Dictionary<K, List<R>> first, Dictionary<K, List<R>> second) {
			foreach (var pair in second) {
				if (first.TryGetValue(pair.Key, out var values)) {
					values.AddRange(pair.Value);
				} else {
					first.Add(pair.Key, pair.Value);
				}
			}
		}

		public void Execute(GeneratorExecutionContext context) {
			using var writer = new StringWriter();
			try {
				// System.Diagnostics.Debugger.Launch();
				var compilation = context.Compilation;

				var commandHandlerSetups = new List<CommandHandlerSetup>();
				var commandInterfaces = new Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>>(SymbolEqualityComparer.Default);
				foreach (var syntaxTree in compilation.SyntaxTrees) {
					var semanticModel = compilation.GetSemanticModel(syntaxTree);
					var walker = new MessagingCodeGenSyntaxWalker(semanticModel);
					walker.Visit(syntaxTree.GetRoot());
					Combine(commandInterfaces, walker.CommandInterfaces);
					commandHandlerSetups.AddRange(walker.CommandHandlers);
				}
				if (!commandInterfaces.Any() && !commandHandlerSetups.Any()) {
					return;
				}
				foreach (var candidate in commandInterfaces) {
					var codeStack = new CodeStack();
					using (codeStack.NewScope(new CompilationUnitBuilder())) {
						codeStack.With(new UsingDirectiveNode(Shared.Namespace.System), new UsingDirectiveNode(Shared.Namespace.System_Text_Json_Serialization));
						var namespacesToImport = new List<string>();
						using (codeStack.NewScope(new NamespaceDeclarationBuilder(candidate.Key.ContainingNamespace.ToDisplayString()))) {
							using (codeStack.NewScope(new InterfaceDeclarationBuilder(candidate.Key.Name).Public().Partial())) {
								var attributeNames = new List<string>();

								foreach (var item in candidate.Value) {
									if (!attributeNames.Contains(item.Name)) {
										namespacesToImport.Add(item.ContainingNamespace.ToDisplayString());
										attributeNames.Add(item.Name);
										using (codeStack.NewScope(new AttributeBuilder("JsonDerivedType"))) {
											codeStack.Begin(new AttributeArgumentListBuilder())
												.With(SyntaxFactory.TypeOfExpression(SyntaxFactory.ParseTypeName(item.Name)))
												.With(new LiteralNode(item.Name))
											.End();
										}
									} else {
										string text = $"Interface {candidate.Key.Name} has two class implementations of the same name {item.Name}";
										var descriptor = new DiagnosticDescriptor("CmdInterfaceCodeGen02", "Command Interface CodeGen", text, "Generator", DiagnosticSeverity.Error, true);
										context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
									}
								}
							}
						}
						foreach (var namespaceToImport in namespacesToImport) {
							codeStack.With(new UsingDirectiveNode(namespaceToImport));
						}
					}
					var code = codeStack.Build();
					writer.WriteSourceHeader(candidate.Key.Name);
					writer.WriteLine(code);
					context.AddSource(candidate.Key.Name, SourceText.From(code, Encoding.UTF8));
				}
				if (commandHandlerSetups.Any()) {
					var codestack = new CodeStack();
					using (codestack.NewScope(new CompilationUnitBuilder())) {
						codestack.With(new UsingDirectiveNode(Shared.Namespace.Microsoft_Extensions_DependencyInjection))
							.With(new UsingDirectiveNode(Shared.Namespace.System))
							.With(new UsingDirectiveNode("Albatross.Messaging.Commands"));
						var myNamespace = commandHandlerSetups.First().CommandHandler.ContainingNamespace.GetFullNamespace();
						using (codestack.NewScope(new NamespaceDeclarationBuilder(myNamespace))) {
							using (codestack.NewScope(new ClassDeclarationBuilder(Shared.Class.CodeGenExtensions).Public().Static())) {
								using (codestack.NewScope(new MethodDeclarationBuilder("IServiceCollection", "RegisterCommands").Public().Static())) {
									codestack.With(new ParameterNode("IServiceCollection", "services").WithThis());
									codestack.With(new ParameterNode(new GenericIdentifierNode("Func", "ulong", "object", "IServiceProvider", "string"), "getQueueName"));
									var commandNames = new HashSet<string>();
									foreach (var item in commandHandlerSetups) {
										using (codestack.NewScope()) {
											codestack.With(new IdentifierNode("services"));
											if (item.ReturnType != null) {
												codestack.With(new GenericIdentifierNode("AddScoped",
													new GenericIdentifierNode("ICommandHandler",
														new TypeNode(item.CommandType.GetTypeNameRelativeToNamespace(myNamespace)),
														new TypeNode(item.ReturnType.GetTypeNameRelativeToNamespace(myNamespace))),
													new TypeNode(item.CommandHandler.GetTypeNameRelativeToNamespace(myNamespace))
												));
											} else {
												codestack.With(new GenericIdentifierNode("AddScoped",
													new GenericIdentifierNode("ICommandHandler",
														new TypeNode(item.CommandType.GetTypeNameRelativeToNamespace(myNamespace))),
													new TypeNode(item.CommandHandler.GetTypeNameRelativeToNamespace(myNamespace))
												));
											}
											codestack.To(new InvocationExpressionBuilder());
										}
										using (codestack.NewScope()) {
											codestack.With(new IdentifierNode("services"));
											if (item.ReturnType != null) {
												codestack.With(new GenericIdentifierNode("AddCommand",
													new TypeNode(item.CommandType.GetTypeNameRelativeToNamespace(myNamespace)),
													new TypeNode(item.ReturnType.GetTypeNameRelativeToNamespace(myNamespace)))
												);
											} else {
												codestack.With(new GenericIdentifierNode("AddCommand",
													new TypeNode(item.CommandType.GetTypeNameRelativeToNamespace(myNamespace)))
												);
											}
											codestack.ToNewBegin(new InvocationExpressionBuilder())
												.Begin(new ArgumentListBuilder())
													.Begin(new NewArrayBuilder("string"))
														.With(item.CommandNames.Select(x => {
															if (commandNames.Contains(x)) {
																string text = $"Command name \"{x}\" is being used by more than 1 command";
																var descriptor = new DiagnosticDescriptor("CmdRegistrationCodeGen01", "Command Registration CodeGen", text, "Generator", DiagnosticSeverity.Error, true);
																context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
															} else {
																commandNames.Add(x);
															}
															return new LiteralNode(x);
														}).ToArray())
													.End()
													.With(new IdentifierNode("getQueueName"))
												.End()
											.End();
										}
									}
									codestack.Begin(new ReturnExpressionBuilder())
										.With(new IdentifierNode("services"))
										.End();
								}
							}
						}
					}
					var code = codestack.Build();
					writer.WriteSourceHeader(Shared.Class.CodeGenExtensions);
					writer.WriteLine(code);
					context.AddSource(Shared.Class.CodeGenExtensions, SourceText.From(code, Encoding.UTF8));
				}
			} catch (Exception err) {
				writer.WriteLine(err.ToString());
				context.CodeGenDiagnostic(DiagnosticSeverity.Error, "CmdInterfaceCodeGen03", err.BuildCodeGeneneratorErrorMessage("messaging"));
			} finally {
				var text = writer.ToString();
				if (!string.IsNullOrEmpty(text)) {
					context.CreateGeneratorDebugFile("albatross-messaging-codegen.debug.txt", text);
				}
			}
		}
		public void Initialize(GeneratorInitializationContext context) { }
	}
}