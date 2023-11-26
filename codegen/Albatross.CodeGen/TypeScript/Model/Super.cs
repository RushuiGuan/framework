using Albatross.CodeGen.Core;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Super : MethodCall  {
		public Super(params ICodeElement[] parameters)
			: base(false, "super", parameters.ToArray()) { }
	}
}
