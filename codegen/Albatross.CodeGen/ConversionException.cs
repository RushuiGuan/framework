using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen {
	public class ConversionException : Exception {
		public ConversionException(string msg) : base(msg) { }
		public ConversionException() { }
	}
}