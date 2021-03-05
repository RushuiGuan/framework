using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Database {
	/// <summary>
	/// A <see cref="Albatross.Database.Table"/> or <see cref="Albatross.Database.View"/> column
	/// </summary>
	public class Column {
		/// <summary>
		/// Column Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Column Type
		/// </summary>
		public SqlType Type { get; set; }

		/// <summary>
		/// If the column can be null
		/// </summary>
		public bool IsNullable { get; set; }

		/// <summary>
		/// If this is an identity column
		/// </summary>
		public bool IsIdentity { get; set; }

		/// <summary>
		/// If this is a computed column
		/// </summary>
		public bool IsComputed { get; set; }

		public bool IsFilestream { get; set; }

		/// <summary>
		/// The ordinal position of the column within the table or view
		/// </summary>
		public int OrdinalPosition { get; set; }
	}
}
