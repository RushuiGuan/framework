using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Text;

namespace Albatross.CodeAnalysis {
	public static class Extensions {
		public static void CodeGenDiagnostic(this GeneratorExecutionContext context, DiagnosticSeverity severity, string code, string message) {
			var descriptor = new DiagnosticDescriptor(code, string.Empty, message, "CodeGenerator", severity, true);
			context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
		}
		public static string BuildCodeGeneneratorErrorMessage(this Exception err, string generatorName) {
			var sb = new StringBuilder($"An exception has been thrown while running the {generatorName} code generator:");
			while (err != null) {
				sb.Append(err.Message);
				err = err.InnerException;
				if (err != null) {
					sb.Append(" -> ");
				}
			}
			return sb.ToString();
		}
		public static void CreateGeneratorDebugFile(this GeneratorExecutionContext context, string fileName, string content) {
			if (context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ProjectDir", out var projectDir)) {
				var path = Path.Combine(projectDir, fileName);
				using (var streamWriter = new StreamWriter(path)) {
					streamWriter.WriteLine(content);
					streamWriter.Flush();
				}
			}
		}
		public static NamespaceDeclarationSyntax CreateNamespaceNullableDirective(this NamespaceDeclarationSyntax syntax) {
			var nullableEnableDirective = SyntaxFactory.Trivia(SyntaxFactory.NullableDirectiveTrivia(SyntaxFactory.Token(SyntaxKind.EnableKeyword), true));
			var nullableDisableDirective = SyntaxFactory.Trivia(SyntaxFactory.NullableDirectiveTrivia(SyntaxFactory.Token(SyntaxKind.DisableKeyword), true));
			return syntax.WithLeadingTrivia(nullableEnableDirective).WithTrailingTrivia(nullableDisableDirective);
		}
	}
}