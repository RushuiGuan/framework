using Albatross.Excel.Table;
using System;

namespace Albatross.Excel.SampleAddIn.Models {
	public class Price {
		public int Id { get; set; }
		public int InstrumentId { get; set; }
		public Instrument Instrument { get; set; } = null!;
		public DateTime Date { get; set; }
		public double Open { get; set; }
		public double High { get; set; }
		public double Low { get; set; }
		public double Close { get; set; }

		public DateTime CreatedUtc { get; set; }
		public string CreatedBy { get; set; } = string.Empty;
	}
}
