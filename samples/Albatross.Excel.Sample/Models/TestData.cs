using Albatross.Excel.Table;
using System;

namespace Albatross.Excel.Sample.Models {
	public class TestData {
		public int Id { get; set; }
		public string Text { get; set; } = string.Empty;
		public int Number { get; set; }
		public DateTime DateTime1 { get; set; }
		public DateTime DateTime2 { get; set; }
		public DateTime DateOnly { get; set; }
	}
}
