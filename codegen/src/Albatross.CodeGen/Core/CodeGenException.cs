using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.Core {
    public class CodeGenException : Exception
    {
		public CodeGenException(string msg) : base(msg) { }
    }
}
