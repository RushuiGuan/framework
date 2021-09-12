using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Writer {
	public class WriteParameters : CodeGeneratorBase<IEnumerable<Parameter>> {
		ICodeGenerator<Parameter> writeParam;

		public WriteParameters(ICodeGenerator<Parameter> writeParam) {
			this.writeParam = writeParam;
		}

		public override void Run(TextWriter writer, IEnumerable<Parameter> source) {
            writer.WriteItems(source, ", ", (w, item) => w.Run(writeParam, item));
		}
	}
}
