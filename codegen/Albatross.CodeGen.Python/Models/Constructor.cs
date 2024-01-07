using System.Collections.Generic;

namespace Albatross.CodeGen.Python.Models {
	public class Constructor : Method {
		public Constructor(string module) : base("__init__", module) { }
		
		public IEnumerable<Field> InitFields => Collection<Field>(nameof(InitFields));
	}
}