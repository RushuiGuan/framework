using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Models {
	public class IfElseCodeBlock : ICodeElement {
		public string ConditionExpression { get; set; }
		public ICodeElement IfContent { get; set; }
		public ICodeElement? ElseContent { get; set; }

		public IfElseCodeBlock(string conditionExpression, ICodeElement ifContent) {
			this.ConditionExpression = conditionExpression;
			this.IfContent = ifContent;
		}

		public TextWriter Generate(TextWriter writer) {
			using (var scope = writer.Append("if (").Append(ConditionExpression).Append(")").BeginScope()) {
				scope.Writer.Code(IfContent);
			}
			if (ElseContent != null) {
				using var elseScope = writer.BeginScope(" else");
				elseScope.Writer.Code(ElseContent);
			}
			writer.WriteLine();
			return writer;
		}
	}
}