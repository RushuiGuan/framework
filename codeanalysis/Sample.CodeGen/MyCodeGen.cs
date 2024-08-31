using Albatross.CodeAnalysis.Syntax;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Sample.CodeGen {
	[Generator]
	public class MyCodeGen : ISourceGenerator {
		public void Execute(GeneratorExecutionContext context) {
			// System.Diagnostics.Debugger.Launch();
			var compilation = context.Compilation;
			var cs = new CodeStack();
			using (cs.Begin(new CompilationUnitBuilder()).NewScope()) {
				using (cs.Begin(new NamespaceDeclarationBuilder("Sample.CodeGen")).NewScope()) {
					using (cs.Begin(new ClassDeclarationBuilder("MyTest")).NewScope()) {
						using (cs.Begin(new MethodDeclarationBuilder("void", "MyMethod")).NewScope()) {
							cs.Begin(new VariableBuilder("string", "test1")).With(new LiteralNode("MyTest".Kebaberize())).End();
						}
					}
				}
			}
			context.AddSource("MyTest", SourceText.From(cs.Build(), Encoding.UTF8));
		}
		public void Initialize(GeneratorInitializationContext context) { }
	}
}
