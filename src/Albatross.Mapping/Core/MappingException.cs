using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Mapping.Core {
	public class MappingException: Exception {
		public MappingException(string msg) : base(msg) { }
	}
}
