using System;

namespace Albatross.CodeGen {
	public class CodeGenException : Exception {
		public CodeGenException(string msg) : base(msg) { }
	}
}