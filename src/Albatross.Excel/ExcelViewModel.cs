using Albatross.Excel.Table;

namespace Albatross.Excel {
	public class ExcelViewModel {
		[ExcelColumn(Hidden = true)]
		public int RowIndex { get; set; }
	}
}
