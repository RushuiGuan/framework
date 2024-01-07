using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class Field : CompositeModuleCodeElement {
		public Field(string name, PythonType type, IModuleCodeElement value) : base(name, string.Empty){
			Type = type;
			this.Value = value;
		}
		public bool Static { get; set; }
		public PythonType Type { 
			get => Single<PythonType>(nameof(Type)); 
			set => Set(value, nameof(Type));
		}
		public IModuleCodeElement Value { 
			get => Single<IModuleCodeElement>(nameof(Value)); 
			set => Set(value, nameof(Value)); 
		}

		public override TextWriter Generate(TextWriter writer) {
			if(!Static) {
				writer.Append(My.Keywords.Self).Append(".");
			}
			writer.Append(Name).Code(Type);
			writer.Append(" = ").Code(Value);
			return writer;
		}
	}
}