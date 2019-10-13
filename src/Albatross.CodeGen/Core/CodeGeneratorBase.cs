using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.Core {
	public abstract class CodeGeneratorBase<T> : ICodeGenerator<T> {
		public abstract void Run(TextWriter writer, T source);

		public void Run(TextWriter writer, object source) {
			this.Run(writer, (T)source);
		}
	}
}
