using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.UnitTest {
	public static class Extension {
		public static string RemoveCarriageReturn(this string text) {
			return text?.Replace("\r", "");
		}
	}
}
