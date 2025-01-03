using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Dto.Classes {
	public abstract class AbstractClass {
		public int Value { get; set; }
	}

	public class DerivedClass : AbstractClass {
		public string Name { get; set; } = string.Empty;
	}
}
