using System.Linq;

namespace Albatross.CodeGen.TypeScript.Models {
	public class Super : MethodCall  {
		public Super(params ICodeElement[] parameters)
			: base(false, "super", parameters.ToArray()) { }
	}
}
