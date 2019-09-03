using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.CSharp.Model {
	[Flags]
	public enum AccessModifier {
		None = 0,
		Public = 1,
		Private = 2,
		Protected = 4,
		Internal = 8,
	}
}
