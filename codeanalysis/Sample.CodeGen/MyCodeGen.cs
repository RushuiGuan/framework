using Albatross.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Sample.CodeGen {
	[Generator]
	public class MyCodeGen : ISourceGenerator {
		public void Execute(GeneratorExecutionContext context) {
			// System.Diagnostics.Debugger.Launch();
			var compilation = context.Compilation;
			var compilationUnit = SyntaxFactory.CompilationUnit();

			var node = new ClassDeclarationBuilder("MyTest").Build([]);
			compilationUnit = compilationUnit.AddMembers((ClassDeclarationSyntax)node);
			var code = compilationUnit.NormalizeWhitespace().ToFullString();
			context.AddSource("MyTest", SourceText.From(code, Encoding.UTF8));
		}
		public void Initialize(GeneratorInitializationContext context) {
		}
	}
}
