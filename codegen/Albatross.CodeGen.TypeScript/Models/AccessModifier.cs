using System;

namespace Albatross.CodeGen.TypeScript.Models {
	[Flags]
	public enum AccessModifier {
		None = 0,
		Public = 1,
		Private = 2,
		Protected = 4,
	}
}
