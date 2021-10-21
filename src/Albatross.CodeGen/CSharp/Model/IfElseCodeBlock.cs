using Albatross.CodeGen.Core;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Model {
	public class IfElseCodeBlock : ICodeBlock {
		public string ConditionExpression { get; set; }
		public ICodeBlock IfContent { get; set; }
		public ICodeBlock? ElseContent { get; set; }

		public IfElseCodeBlock(string conditionExpression, ICodeBlock ifContent) {
			this.ConditionExpression = conditionExpression;
			this.IfContent = ifContent;
		}
	}
	
	public class WriteIfElseCodeBlock : WriteCodeBlock<IfElseCodeBlock> {
		private readonly ICodeGenFactory factory;

		public WriteIfElseCodeBlock(ICodeGenFactory factory) {
			this.factory = factory;
		}

		public override void Run(TextWriter writer, IfElseCodeBlock source) {
			using (var scope = writer.Append("if (").Append(source.ConditionExpression).Append(")").BeginScope()) {
				factory.Get(source.IfContent.GetType()).Run(writer, source);
			}
			if(source.ElseContent != null) {
				using var elseScope = writer.BeginScope(" else ");
				factory.RunCodeGen(elseScope.Writer, source.ElseContent);
			}
		}
	}
}
