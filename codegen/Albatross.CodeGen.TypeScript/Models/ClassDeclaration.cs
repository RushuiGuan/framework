using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class ClassDeclaration : ICodeElement {
		public ClassDeclaration(string name) {
			this.Name = name;
		}
		public string Name { get; set; }

		public AccessModifier AccessModifier { get; set; }
		public ClassDeclaration? BaseClass { get; set; }
		public bool IsGeneric { get; set; }
		public MethodCallExpression? Decorator { get; set; }

		public ConstructorDeclaration? Constructor { get; set; }
		public List<ImportExpression> Imports { get; set; } = new List<ImportExpression>();
		public List<GetterDeclaration> Getters { get; set; } = new List<GetterDeclaration>();
		public List<PropertyDeclaration> Properties { get; set; } = new List<PropertyDeclaration>();
		public List<MethodDeclaration> Methods { get; set; } = new List<MethodDeclaration>();

		public TextWriter Generate(TextWriter writer) {
			if (this.Decorator != null) { writer.Code(this.Decorator).WriteLine(); }
			writer.Append("export ").Append("class ").Append(Name);
			if (BaseClass != null) {
				writer.Append(" extends ").Append(BaseClass.Name);
			}
			using (var scope = writer.BeginScope()) {
				foreach (var getter in Getters) {
					scope.Writer.Code(getter);
				}
				foreach (var property in Properties) {
					scope.Writer.Code(property);
				}
				if (Constructor != null) {
					scope.Writer.Code(Constructor);
				}
				foreach (var method in Methods) {
					scope.Writer.Code(method);
				}
			}
			writer.WriteLine();
			return writer;
		}
	}
}
