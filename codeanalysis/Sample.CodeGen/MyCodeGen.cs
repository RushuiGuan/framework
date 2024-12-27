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
			using (cs.NewScope(new CompilationUnitBuilder())) {
				using (cs.NewScope(new NamespaceDeclarationBuilder("Sample.CodeGen"))) {
					using (cs.NewScope(new ClassDeclarationBuilder("MyTest").Public())) {
						using (cs.NewScope(new MethodDeclarationBuilder("void", "MyMethod").Public())) {
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
