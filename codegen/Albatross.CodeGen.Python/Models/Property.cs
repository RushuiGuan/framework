using System;
using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class Property : CompositeModuleCodeElement {
		public Property(string name, PythonType type, IModuleCodeElement value) : base(name, string.Empty) {
			Type = type;
			Value = value;
		}

		public PythonType Type {
			get => this.Single<PythonType>(nameof(Type));
			set => Set(value, nameof(Type));
		}
		public IModuleCodeElement Value {
			get => this.Single<IModuleCodeElement>(nameof(Value));
			set => Set(value, nameof(Value));
		}

		public override TextWriter Generate(TextWriter writer) {
			throw new NotImplementedException();
		}
	}
}