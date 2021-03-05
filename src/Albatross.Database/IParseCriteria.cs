using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database {
	public interface IParseCriteria {
		void Parse(string criteria, out string schema, out string name);
	}
}
