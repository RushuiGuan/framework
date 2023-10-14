using Albatross.Excel.Table;

namespace Albatross.Excel.Sample.Models {
	public class Instrument {
		public Instrument(string name, string ticker) {
			Name = name;
			Ticker = ticker;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Ticker { get; set; }
	}
}
