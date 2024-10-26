using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Albatross.CodeAnalysis.Syntax {
	public class PropertyNode : NodeContainer {
		public PropertyNode(TypeNode typeNode, string name) : base(SyntaxFactory.PropertyDeclaration(typeNode.Type, name)) { }
		public PropertyNode(string type, string name) : this(new TypeNode(type), name) { }

		public PropertyNode Public() {
			Node = ((PropertyDeclarationSyntax)Node).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			return this;
		}
		public PropertyNode Private() {
			Node = ((PropertyDeclarationSyntax)Node).AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
			return this;
		}
		public PropertyNode Protected() {
			Node = ((PropertyDeclarationSyntax)Node).AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword));
			return this;
		}

		public PropertyDeclarationSyntax PropertyDeclaration => (PropertyDeclarationSyntax)Node;

		/// <summary>
		/// Create a public property with its getter and setter both as public
		/// </summary>
		public PropertyNode Default() {
			// Create the get accessor
			var getAccessor = SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
				.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

			// Create the set accessor
			var setAccessor = SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
				.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

			this.Node = this.PropertyDeclaration
				.WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List(new[] { getAccessor, setAccessor })))
				.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			return this;
		}

		/// <summary>
		/// Create a public property with its getter only
		/// </summary>
		public PropertyNode GetterOnly() {
			// Create the get accessor
			var getAccessor = SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
				.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

			this.Node = this.PropertyDeclaration
				.WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List(new[] { getAccessor })))
				.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
			return this;
		}
	}
}