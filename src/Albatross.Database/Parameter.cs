using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database
{
	/// <summary>
	/// The class represents a stored procedure parameter
	/// </summary>
    public class Parameter {
		public string Name { get; set; }
		public string Mode{ get; set; }	
		public bool IsResult { get; set; }
		public int OrdinalPosition { get; set; }

		public SqlType Type { get; set; }
	}
}
