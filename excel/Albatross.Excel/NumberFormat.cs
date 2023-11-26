using ExcelDna.Integration;
using System;
using System.Text;

namespace Albatross.Excel {
	public record class NumberFormat :CellProperty{
		public const string StandardDateOnlyFormat = "yyyy-mm-dd";
		public const string StandardDateTimeFormat = "yyyy-mm-dd hh:mm:ss AM/PM";
		public const string StandardNumberFormat = "#,#0";

		object format = ExcelMissing.Value;
		bool delete = false;
		
		public bool HasValue => format != ExcelMissing.Value;
		public void Format(string? format) {
			if (string.IsNullOrEmpty(format)) {
				this.DeleteFormat();
			} else {
				this.format = format;
				this.delete = false;
			}
		}
		public void StandardNumber(int decimalPlaces = 0) {
			var sb = new StringBuilder(StandardNumberFormat);
			if (decimalPlaces > 0) {
				sb.Append('.').Append('0', decimalPlaces);
			}
			Format(sb.ToString());
		}
		public void StandardDate() {
			Format(StandardDateOnlyFormat);
		}
		public void StandardDateTime() {
			Format(StandardDateTimeFormat);
		}
		public void Percent(int decimalPlaces) {
			var sb = new StringBuilder("0");
			if(decimalPlaces > 0) {
				sb.Append('.');
				sb.Append('0', decimalPlaces);
			}
			sb.Append('%');
			Format(sb.ToString());
		}
		public override void Apply(ExcelReference range) {
			if(this.HasValue) {
				XlCall.Excel(XlCall.xlcSelect, range);
				XlCall.Excel(XlCall.xlcFormatNumber, this.format);
			}else if (delete) {
				XlCall.Excel(XlCall.xlcSelect, range);
				XlCall.Excel(XlCall.xlcDeleteFormat);
			}
		}
		public void DeleteFormat() {
			this.format = ExcelMissing.Value;
			this.delete = true;
		}
	}
}
