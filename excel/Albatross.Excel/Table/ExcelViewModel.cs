namespace Albatross.Excel.Table {
	public class ExcelViewModel {
		[ExcelColumn(Hidden = true)]
		public int RowIndex { get; set; }
	}
}