namespace Albatross.Excel.Table {
	public record class ReadError {
		public int RowIndex { get; set; }
		public int ColumnIndex { get; set; }
		public string Error { get; set; }
		public ReadError(int rowIndex, int columnIndex, string error) {
			RowIndex = rowIndex;
			ColumnIndex = columnIndex;
			Error = error;
		}
	}
}