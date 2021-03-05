using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database {
	/// <summary>
	/// The class represents a database data type
	/// </summary>
	public class SqlType : IEquatable<SqlType> {
		public string Schema { get; set; }
		public string Name { get; set; }
		public int? MaxLength { get; set; }
		public int? Precision { get; set; }
		public int? Scale { get; set; }
		public bool IsNullable { get; set; }
		public bool IsUserDefined { get; set; }
		public bool IsTableType { get; set; }

		public bool Equals(SqlType other) {
			if (other != null) {
				return Schema == other.Schema &&
					Name == other.Name &&
					MaxLength == other.MaxLength &&
					Precision == other.Precision &&
					Scale == other.Scale &&
					IsNullable == other.IsNullable &&
					IsUserDefined == other.IsUserDefined &&
					IsTableType == other.IsTableType;
			} else {
				return false;
			}
		}
	}
}
