using Albatross.CodeAnalysis.Symbols;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.EFCore.CodeGen {
	/// <summary>
	/// Find any concrete class that implements <see cref="Albatross.EFCore.IBuildEntityModel" /> interface
	/// </summary>
	public class EntityModelClassWalker : CSharpSyntaxWalker {
		private readonly Compilation compilation;
		private readonly SemanticModel semanticModel;
		public List<INamedTypeSymbol> EntityModelBuilderClasses { get; } = new List<INamedTypeSymbol>();
		public INamedTypeSymbol? DbSessionClass { get; private set; }

		public EntityModelClassWalker(Compilation compilation, SemanticModel semanticModel) {
			this.compilation = compilation;
			this.semanticModel = semanticModel;
		}
		public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
			FindTargets(node);
			base.VisitClassDeclaration(node);
		}
		public override void VisitRecordDeclaration(RecordDeclarationSyntax node) {
			FindTargets(node);
			base.VisitRecordDeclaration(node);
		}

		void FindTargets(TypeDeclarationSyntax node) {
			if (node.Modifiers.Any(SyntaxKind.PublicKeyword)) {
				var classSymbol = semanticModel.GetDeclaredSymbol(node);
				if (classSymbol != null && classSymbol.IsConcreteClass()) {
					if (classSymbol.AllInterfaces.Any(x => x.GetFullName() == My.EntityModelBuilderInterfaceName
						&& (!x.Constructors.Any() || x.Constructors.Any(x => x.Parameters.Length == 0)))) {
						EntityModelBuilderClasses.Add(classSymbol);
					} else if (classSymbol.IsDerivedFrom(My.DbSessionClassName)) {
						DbSessionClass = classSymbol;
					}
				}
			}
		}
	}
}