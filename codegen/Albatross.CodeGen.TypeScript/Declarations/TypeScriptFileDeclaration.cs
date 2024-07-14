using Albatross.Collections;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Declarations {
	public record class TypeScriptFileDeclaration : SyntaxNode, IDeclaration, ICodeElement {
		public TypeScriptFileDeclaration(string name) {
			this.Identifier = new IdentifierNameExpression(name);
		}
		public string FileName => $"{Identifier.Name}.ts";
		public IdentifierNameExpression Identifier { get; }
		public IEnumerable<IModifier> Modifiers { get; init; } = [];
		public IEnumerable<ImportExpression> ImportDeclarations { get; init; } = [];
		public IEnumerable<EnumDeclaration> EnumDeclarations { get; init; } = [];
		public IEnumerable<InterfaceDeclaration> InterfaceDeclarations { get; init; } = [];
		public IEnumerable<ClassDeclaration> ClasseDeclarations { get; init; } = [];

		public override IEnumerable<ISyntaxNode> Children => new List<ISyntaxNode> { Identifier }
				.UnionAll(ImportDeclarations, EnumDeclarations, InterfaceDeclarations, ClasseDeclarations);

		public override TextWriter Generate(TextWriter writer) {
			foreach (var item in ImportDeclarations) {
				writer.Code(item);
			}
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
