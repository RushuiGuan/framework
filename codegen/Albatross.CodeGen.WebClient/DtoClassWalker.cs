using Albatross.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.WebClient {
	public class DtoClassWalker : CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		public List<INamedTypeSymbol> Result { get; } = new List<INamedTypeSymbol>();

		public DtoClassWalker(SemanticModel semanticModel) {
			this.semanticModel = semanticModel;
		}

		bool IsValidDtoType([NotNullWhen(true)] INamedTypeSymbol? symbol) =>
			symbol != null
				&& symbol.DeclaredAccessibility == Accessibility.Public
				&& !symbol.IsAbstract
				&& !symbol.IsAnonymousType
				&& !symbol.IsStatic
				&& symbol.TypeKind != TypeKind.Interface
				&& symbol.TypeKind != TypeKind.Enum
				&& !symbol.IsDerivedFrom(semanticModel.Compilation.GetRequiredSymbol("System.Text.Json.Serialization.JsonConverter"));

		public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
			var symbol = semanticModel.GetDeclaredSymbol(node);
			if (IsValidDtoType(symbol)) {
				Result.Add(symbol);
			}
			base.VisitClassDeclaration(node);
		}

		public override void VisitRecordDeclaration(RecordDeclarationSyntax node) {
			var symbol = semanticModel.GetDeclaredSymbol(node);
			if (IsValidDtoType(symbol)) {
				Result.Add(symbol);
			}
			base.VisitRecordDeclaration(node);
		}
	}
}
