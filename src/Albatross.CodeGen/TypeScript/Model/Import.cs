using Albatross.CodeGen.Core;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Import : ICodeElement {
		public Import(string source, params string[] items) {
			this.Source = source;
			foreach (var item in items) {
				this.Items.Add(item);
			}
		}

		public ISet<string> Items { get; set; } = new HashSet<string>();
		public string Source { get; set; }

		// import {format, parse} from 'date-fns';
		public TextWriter Generate(TextWriter writer) {
			writer.Append("import ");
			writer.Append("{ ").WriteItems(Items, ", ").Append(" } ");
			writer.Append(" from ").StringLiteral(Source, singleQuote: true).Semicolon().WriteLine();
			return writer;
		}

		public static IEnumerable<Import> Consolidate(IEnumerable<Import> imports) {
			var results = imports.GroupBy(args => args.Source).Select(args => {
				var result = new Import(args.Key);
				foreach (var item in args.SelectMany(x => x.Items)) {
					result.Items.Add(item);
				}
				return result;
			}).ToArray();
			return results;
		}
	}
}
