using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeAnalysis.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Albatross.CodeAnalysis.MSBuild {
	public static class Extensions {
		/// <summary>
		/// Create scoped registration for the <see cref="Microsoft.CodeAnalysis.Compilation"/> instance of specified project file so that it can be
		/// injected as dependencies for other classes.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="projectFile"></param>
		/// <returns></returns>
		public static IServiceCollection AddMSBuildProject(this IServiceCollection services, string projectFile) {
			services.AddScoped(provider => MSBuildWorkspace.Create());
			services.AddScoped<ICurrentProject>(provider => new CurrentProject(projectFile));
			services.AddScoped<ICompilationFactory, MSBuildProjectCompilationFactory>();
			services.AddScoped<Compilation>(provider => provider.GetRequiredService<ICompilationFactory>().Create());
			return services;
		}
		const string text = @"class MyClass
{
    void MyMethod()
    {
        if (true)
        {
            Console.WriteLine(""""Hello, World!"""");
        }
    }
}";
		public static string BuildWithFormat(this CodeStack stack) {
			using var workspace = new AdhocWorkspace();
			var options = workspace.Options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForCatch, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForClausesInQuery, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInProperties, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAccessors, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousMethods, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInControlBlocks, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousTypes, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInObjectCollectionArrayInitializers, false)
				.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInLambdaExpressionBody, false)

				.WithChangedOption(CSharpFormattingOptions.NewLineForElse, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForCatch, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForFinally, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForFinally, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForMembersInObjectInit, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForMembersInAnonymousTypes, false)
				.WithChangedOption(CSharpFormattingOptions.NewLineForClausesInQuery, false)
				.WithChangedOption(FormattingOptions.UseTabs, LanguageNames.CSharp, true)
				.WithChangedOption(FormattingOptions.TabSize, LanguageNames.CSharp, 4);

			var sb = new StringBuilder();
			foreach (var node in stack.Finalize()) {
				if (node is INodeContainer container) {
					var formatted = Formatter.Format(container.Node.NormalizeWhitespace(), workspace, options);
					sb.AppendLine(formatted.ToFullString());
				} else {
					throw new InvalidOperationException($"Stack item of type {node.GetType().Name} is not expected.  Only {typeof(INodeContainer).Name} is expected");
				}
			}
			return sb.ToString();
		}

		public static IEnumerable<MetadataReference> GetGlobalReferences() {
			var assemblies = new[] {
				/*Making sure all MEF assemblies are loaded*/
				typeof(System.Composition.Convention.AttributedModelProvider).Assembly, //System.Composition.AttributeModel
				typeof(System.Composition.Convention.ConventionBuilder).Assembly,   //System.Composition.Convention
				typeof(System.Composition.Hosting.CompositionHost).Assembly,        //System.Composition.Hosting
				typeof(System.Composition.CompositionContext).Assembly,             //System.Composition.Runtime
				typeof(System.Composition.CompositionContextExtensions).Assembly,   //System.Composition.TypedParts
				typeof(Enumerable).Assembly,										//System.Linq
				typeof(System.Text.Json.JsonDocument).Assembly,						//System.Text.Json
			};
			var result = new List<MetadataReference>(from assembly in assemblies select MetadataReference.CreateFromFile(assembly.Location));
			var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location) ?? throw new System.Exception("Cannot find assembly path for mscorlib.dll");
			result.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
			result.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")));
			result.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")));
			result.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")));
			result.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll")));
			return result;
		}

		public static Compilation CreateCompilation(this string code) {
			var syntaxTree = CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(LanguageVersion.Default));
			var references = GetGlobalReferences();
			var compilation = CSharpCompilation.Create("TestCompilation", [syntaxTree], references,
				new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			return compilation;
		}
	}
}