using Albatross.Collections;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Declarations {
	public record class TypeScriptFileDeclaration : SyntaxNode, IDeclaration, ICodeElement {
		public TypeScriptFileDeclaration(string name) {
			this.Name = name;
		}
		public string FileName => $"{Name}.ts";
		public string Name { get; }
		public IEnumerable<IModifier> Modifiers { get; init; } = [];
		public IEnumerable<ImportExpression> ImportDeclarations { get; init; } = [];
		public IEnumerable<EnumDeclaration> EnumDeclarations { get; init; } = [];
		public IEnumerable<InterfaceDeclaration> InterfaceDeclarations { get; init; } = [];
		public IEnumerable<ClassDeclaration> ClasseDeclarations { get; init; } = [];

		public override IEnumerable<ISyntaxNode> Children => ImportDeclarations.Cast<ISyntaxNode>()
			.Union(EnumDeclarations)
			.Union(InterfaceDeclarations)
			.Union(ClasseDeclarations);


		public override TextWriter Generate(TextWriter writer) {
			var importExpressions = this.ImportDeclarations.Union(new ImportCollection(this.GetDescendants()));
			new ImportCollection(importExpressions).Generate(writer);
			writer.WriteLine();
			foreach (var item in EnumDeclarations) {
				writer.Code(item);
			}
			foreach (var item in InterfaceDeclarations) {
				writer.Code(item);
			}
			foreach (var item in ClasseDeclarations) {
				writer.Code(item);
			}
			return writer;
		}
	}
}
