using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class IdentifierNameExpression : SyntaxNode, IIdentifierNameExpression {
		public IdentifierNameExpression(string name) {
			if (Defined.Patterns.IdentifierName.IsMatch(name)) {
				this.Name = name;
			} else {
				throw new ArgumentException($"Invalid identifier name {name}");
			}
		}
		public override IEnumerable<ISyntaxNode> Children => [];
		public string Name { get; }
		public override TextWriter Generate(TextWriter writer) {
			writer.Append(Name);
			return writer;
		}
	}
}