using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Assignment : CompositeModuleCodeElement {
		public Assignment(string name, IModuleCodeElement expression) : base(string.Empty, string.Empty) {
			this.Variable = new Variable(name);
			Expression = expression;
		}

		public Variable Variable { 
			get => Single<Variable>(nameof(Variable)); 
			set => Set(value, nameof(Variable));
		}
		public IModuleCodeElement Expression { 
			get => Single<IModuleCodeElement>(nameof(Expression));
			set	=> Set(value, nameof(Expression));
		}
		public override TextWriter Generate(TextWriter writer) => writer.Code(Variable).Append(" = ").Code(Expression);
	}
}