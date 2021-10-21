using Albatross.CodeGen.Core;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Import : ICodeElement {
		public Import(TypeScriptFile file) {
			this.From = file;
		}
		public ISet<string> Items { get; set; } = new HashSet<string>();
		public TypeScriptFile From { get; set; }

		// import {format, parse} from 'date-fns';
		public TextWriter Generate(TextWriter writer) {
			writer.Append("import ");
			writer.Append("{ ").WriteItems(Items, ", ").Append(" } ");
			writer.Append(" from ").StringLiteral("./" + From.Name, singleQuote: true).Semicolon().WriteLine();
			return writer;
		}
	}
}
