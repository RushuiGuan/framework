using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.CodeGen.Core {
	public interface ICodeGenerator<in T> : ICodeGenerator {
		void Run(TextWriter writer, T source);
	}

	public interface ICodeGenerator {
        void Run(TextWriter writer, object source);
	}
}
