using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class ModuleSourceExpression : SyntaxNode, ISourceExpression {
		// this regex is more restrictive than the actual module name regex
		public ModuleSourceExpression(string name) {
			if (Defined.Patterns.ModuleSource.IsMatch(name)) {
				this.ModuleName = name;
			} else {
				throw new ArgumentException($"Invalid module name {name}");
			}
		}
		public string ModuleName { get; }

		public override IEnumerable<ISyntaxNode> Children => [];

		public override TextWriter Generate(TextWriter writer) {
			writer.Append('"').Append(ModuleName).Append('"');
			return writer;
		}
		public override string ToString() => this.ModuleName;
	}
}