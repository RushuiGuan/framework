using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Return : CompositeModuleCodeElement {
		public Return(IModuleCodeElement expression) : base(string.Empty, string.Empty) {
			Expression = expression;
		}

		public IModuleCodeElement Expression {
			get => Single<IModuleCodeElement>(nameof(Expression));
			set => Set(value, nameof(Expression));
		}
		public override TextWriter Generate(TextWriter writer)
			=> writer.AppendLine().Append("return ").Code(Expression);
	}
}