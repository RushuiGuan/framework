using Albatross.CodeGen.Python.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Albatross.CodeGen.Python.Conversions {
	public class ConvertCSharpEnumDeclarationSyntaxToClassDeclaration : IConvertObject<EnumDeclarationSyntax, Models.ClassDeclaration> {
		private readonly SemanticModel semanticModel;

		public ConvertCSharpEnumDeclarationSyntaxToClassDeclaration(SemanticModel semanticModel) {
			this.semanticModel = semanticModel;
		}

		public Models.ClassDeclaration Convert(EnumDeclarationSyntax syntax) {
			Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax classDeclaration;
			var model = new Models.ClassDeclaration(syntax.Identifier.Text)
				.AddBaseClass(My.Classes.Enum());

			foreach (var member in syntax.Members) {
				var value = this.semanticModel.GetDeclaredSymbol(member)?.ConstantValue
					?? throw new InvalidOperationException($"Enum {member.Identifier.Text} is missing value");
				model.AddField(new Field(member.Identifier.Text, My.Types.NoType(), new Literal(value)) {
					Static = true,
				});
			}
			return model;
		}
		object IConvertObject<EnumDeclarationSyntax>.Convert(EnumDeclarationSyntax from) => this.Convert(from);
	}
}