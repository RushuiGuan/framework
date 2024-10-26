using System;

namespace Albatross.Excel.Table {
	[AttributeUsage(AttributeTargets.Property)]
	public class ExcelColumnAttribute : Attribute {
		public string? Title { get; set; }
		public bool ReadOnly { get; set; }
		public bool Required { get; set; }
		public bool Hidden { get; set; }
	}
}