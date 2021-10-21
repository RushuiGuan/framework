using Albatross.CodeGen.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Model {
	public class ForEachCodeBlock : ICodeBlock {
		public ForEachCodeBlock(string itemVariable, string collectionVariable) {
			ItemVariable = itemVariable;
			CollectionVariable = collectionVariable;
		}
		public string ItemVariable { get; set; }
		public string CollectionVariable { get; set; }
		public ICodeBlock ForEachContent { get; set; } = new CSharpCodeBlock();
	}

	public class WriteForEachCodeBlock : WriteCodeBlock<ForEachCodeBlock> {
		private readonly ICodeGenFactory factory;

		public WriteForEachCodeBlock(ICodeGenFactory factory) {
			this.factory = factory;
		}

		public override void Run(TextWriter writer, ForEachCodeBlock source) {
			using var scope = writer.Append("foreach(")
				.Append("var ").Append(source.ItemVariable).Append(" in ").Append(source.CollectionVariable)
				.BeginScope();
			factory.RunCodeGen(scope.Writer, source);
		}
	}
}
