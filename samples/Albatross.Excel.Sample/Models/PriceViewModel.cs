using Albatross.Excel.Table;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.Excel.Sample.Models {
	public class PriceViewModel : ExcelViewModel {
		public int? Id { get; set; }
		public int InstrumentId { get; set; }
		public DateTime Date { get; set; }
		public double? Open { get; set; }
		public double? High { get; set; }
		public double? Low { get; set; }
		public double? Close { get; set; }
		[ExcelColumn(ReadOnly = true)]
		public string? CreatedBy { get; set; }
		[ExcelColumn(ReadOnly = true)]
		public DateTime? CreatedUtc { get; set; }

		public PriceViewModel() { }

		public PriceViewModel(Price src) {
			Set(src);
		}
		public PriceViewModel(Instrument instrument) {
			this.InstrumentId = instrument.Id;
		}
		public bool TryCreatePrice([NotNullWhen(true)] out Price? price) {
			if (Open != null && High != null && Low != null && Close != null) {
				price = new Price {
					InstrumentId = this.InstrumentId,
					Date = this.Date,
					Open = this.Open.Value,
					High = this.High.Value,
					Low = this.Low.Value,
					Close = this.Close.Value,
				};
				return true;
			} else {
				price = null;
				return false;
			}
		}

		public void Set(Price src) {
			this.Id = src.Id;
			this.InstrumentId = src.InstrumentId;
			this.Open = src.Open;
			this.High = src.High;
			this.Low = src.Low;
			this.Close = src.Close;
			this.CreatedBy = src.CreatedBy;
			this.CreatedUtc = src.CreatedUtc;
		}
	}
}