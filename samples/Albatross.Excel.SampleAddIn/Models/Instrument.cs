using Albatross.Excel.Table;
using System;

namespace Albatross.Excel.SampleAddIn.Models {
	public class Instrument {
		public Instrument(string name, string ticker) {
			Name = name;
			Ticker = ticker;
		}

		public DateTime Date { get; set; }
		public string Name { get; set; }
		public string Ticker { get; set; }
		public string? Description { get; set; }
		public int Id { get; set; }
		public int Number { get; set; }
	}
}
