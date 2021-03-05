using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// The class represents a sql variable
	/// </summary>
    public class Variable {
		public string Name { get; set; }
		public SqlType Type { get; set; }
	}
}
