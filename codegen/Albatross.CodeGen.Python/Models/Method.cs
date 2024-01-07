using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Method : CompositeModuleCodeElement {
		public Method(string name) : this(name, string.Empty) { }
		public Method(string name, string module) : base(name, module) {
			Parameters = new ParameterCollection();
			ReturnType = My.Types.AnyType();
			CodeBlock = new Pass();
		}
		public bool IsStatic { get; set; }
		public IEnumerable<Decorator> Decorators => Collection<Decorator>(nameof(Decorators));
		public ParameterCollection Parameters {
			get => Single<ParameterCollection>(nameof(Parameters));
			set => Set(value, nameof(Parameters));
		}
		public IModuleCodeElement CodeBlock {
			get => Single<IModuleCodeElement>(nameof(CodeBlock));
			set => Set(value, nameof(CodeBlock));
		}
		public PythonType ReturnType {
			get => Single<PythonType>(nameof(ReturnType));
			set => Set(value, nameof(ReturnType));
		}

		public override TextWriter Generate(TextWriter writer) {
			foreach (var decorator in Decorators) {
				writer.Code(decorator).WriteLine();
			}
			writer.Append(My.Keywords.Def).Space().Append(Name).OpenParenthesis().Code(Parameters).CloseParenthesis();
			using (var scope = writer.BeginPythonScope()) {
				scope.Writer.Code(BuildBody());
			}
			writer.WriteLine();
			return writer;
		}
		public virtual ICodeElement BuildBody() => this.CodeBlock;
	}
}