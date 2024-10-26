using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.WebClient.Settings;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.WebClient {
	public class DtoClassEnumWalker : CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		public List<INamedTypeSymbol> DtoClasses { get; } = new List<INamedTypeSymbol>();
		public List<INamedTypeSymbol> EnumTypes { get; } = new List<INamedTypeSymbol>();
		Settings.SymbolFilter filter;

		public DtoClassEnumWalker(SemanticModel semanticModel, SymbolFilterPatterns patterns) {
			this.semanticModel = semanticModel;
			filter = new Settings.SymbolFilter(patterns);
		}

		bool IsValidDtoClass([NotNullWhen(true)] INamedTypeSymbol? symbol) =>
			symbol != null
				&& symbol.DeclaredAccessibility == Accessibility.Public
				&& !symbol.IsAbstract
				&& !symbol.IsAnonymousType
				&& !symbol.IsStatic
				&& symbol.TypeKind != TypeKind.Delegate
				&& !symbol.IsGenericTypeDefinition()
				&& symbol.TypeKind != TypeKind.Interface
				&& symbol.TypeKind != TypeKind.Enum
				&& !symbol.IsDerivedFrom("System.Text.Json.Serialization.JsonConverter");

		public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
			var symbol = semanticModel.GetDeclaredSymbol(node);
			if (IsValidDtoClass(symbol) && this.filter.ShouldKeep(symbol.GetFullName())) {
				DtoClasses.Add(symbol);
			}
			base.VisitClassDeclaration(node);
		}

		public override void VisitRecordDeclaration(RecordDeclarationSyntax node) {
			var symbol = semanticModel.GetDeclaredSymbol(node);
			if (IsValidDtoClass(symbol) && this.filter.ShouldKeep(symbol.GetFullName())) {
				DtoClasses.Add(symbol);
			}
			base.VisitRecordDeclaration(node);
		}
		public override void VisitEnumDeclaration(EnumDeclarationSyntax node) {
			var symbol = semanticModel.GetDeclaredSymbol(node);
			if (symbol != null && symbol.DeclaredAccessibility == Accessibility.Public && filter.ShouldKeep(symbol.GetFullName())) {
				EnumTypes.Add(symbol);
			}
			base.VisitEnumDeclaration(node);
		}
	}
}