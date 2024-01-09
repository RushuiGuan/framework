using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Method : CompositeModuleCodeElement {
		public Method(string name) : this(name, string.Empty) { }
		public Method(string name, string module) : base(name, module) {
			Parameters = new ParameterCollection();
			ReturnType = My.Types.NoType();
			CodeBlock = new CompositeModuleCodeBlock();
		}
		public bool Static { get; set; }
		public IEnumerable<Decorator> Decorators => Collection<Decorator>(nameof(Decorators));
		public void AddDecorator(Decorator decorator) => AddCodeElement(decorator, nameof(Decorators));
		public void RemoveDecorator(Decorator decorator) => RemoveCodeElement(decorator, nameof(Decorators));

		public ParameterCollection Parameters {
			get => Single<ParameterCollection>(nameof(Parameters));
			set => Set(value, nameof(Parameters));
		}
		public CompositeModuleCodeElement CodeBlock {
			get => Single<CompositeModuleCodeElement>(nameof(CodeBlock));
			set => Set(value, nameof(CodeBlock));
		}
		public PythonType ReturnType {
			get => Single<PythonType>(nameof(ReturnType));
			set => Set(value, nameof(ReturnType));
		}

		public override void Build() {
			if (this.Static) {
				AddDecorator(My.Decorators.StaticMethod());
			} else {
				this.Parameters.Insert(0, new Variable(My.Keywords.Self, My.Types.NoType()));
			}
			this.ReturnType.MethodReturnType = true;
			base.Build();
		}

		public override TextWriter Generate(TextWriter writer) {
			foreach (var decorator in Decorators) {
				writer.Code(decorator);
			}
			writer.AppendLine().Append(My.Keywords.Def).Space().Append(Name)
				.OpenParenthesis().Code(Parameters).CloseParenthesis()
				.Code(ReturnType);

			using (var scope = writer.BeginPythonScope()) {
				if (!CodeBlock.Any()) { CodeBlock.Add(new Pass()); }
				scope.Writer.Code(CodeBlock);
			}
			return writer;
		}
	}
}