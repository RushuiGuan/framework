using interop = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.Excel {
	public static class My {
		public const int MaxRowIndex = 1048575;
		public const int MaxColumnIndex = 16383;
		public static interop.Workbook ActiveWorkbook(string? sheetName = null) {
			interop.Application app = (interop.Application)ExcelDnaUtil.Application;
			interop.Workbook? book = app.ActiveWorkbook;
			if (book == null) {
				book = app.Workbooks.Add();
				if (!string.IsNullOrEmpty(sheetName)) {
					((interop.Worksheet)book.ActiveSheet).Name = sheetName;
				}
			}
			return book;
		}
		public static interop.Worksheet ActiveWorksheet(string? sheetName = null) {
			interop.Application app = (interop.Application)ExcelDnaUtil.Application;
			interop.Workbook? book = app.ActiveWorkbook;
			if (book == null) {
				book = app.Workbooks.Add();
				if (!string.IsNullOrEmpty(sheetName)) {
					((interop.Worksheet)book.ActiveSheet).Name = sheetName;
				}
			}
			return book.ActiveSheet;
		}
		public static ExcelReference? ActiveSelection() => (ExcelReference)XlCall.Excel(XlCall.xlfSelection);
		public static void Select(this ExcelReference reference) => XlCall.Excel(XlCall.xlcSelect, reference);

		public static interop.Worksheet GetOrCreateSheet(this interop.Workbook book, string name, bool activate = true) {
			interop.Worksheet? sheet = null;
			for (int i = 1; i <= book.Sheets.Count; i++) {
				var item = (interop.Worksheet)book.Sheets[i];
				if (item.Name == name) {
					sheet = item;
					break;
				}
			}
			if (sheet == null) {
				sheet = book.Sheets.Add();
				sheet.Name = name;
			}
			if (activate) { sheet.Activate(); }
			return sheet;
		}
		public static bool TryGetSheet(this interop.Workbook book, string name, [NotNullWhen(true)] out interop.Worksheet? sheet, bool activate = true) {
			for (int i = 1; i <= book.Sheets.Count; i++) {
				var item = (interop.Worksheet)book.Sheets[i];
				if (item.Name == name) {
					sheet = item;
					if (activate) { sheet.Activate(); }
					return true;
				}
			}
			sheet = null;
			return false;
		}
		public static bool TryFindLastColumn(this ExcelReference cell, [NotNullWhen(true)] out int? lastColumnIndex) {
			object cellValue = new ExcelReference(cell.RowFirst, cell.ColumnFirst).GetValue();
			XlCall.Excel(XlCall.xlcSelect, cell);
			XlCall.Excel(XlCall.xlcSelectEnd, (int)Direction.Right);
			var value = ((ExcelReference)XlCall.Excel(XlCall.xlfSelection)).ColumnLast;
			if (value == My.MaxColumnIndex) {
				if (cellValue == ExcelEmpty.Value) {
					lastColumnIndex = null;
					return false;
				} else {
					lastColumnIndex = cell.ColumnFirst;
					return true;
				}
			} else {
				lastColumnIndex = value;
				return true;
			}
		}
		public static bool TryFindLastRow(this ExcelReference cell, [NotNullWhen(true)] out int? lastRowIndex) {
			object cellValue = new ExcelReference(cell.RowFirst, cell.ColumnFirst).GetValue();
			XlCall.Excel(XlCall.xlcSelect, cell);
			XlCall.Excel(XlCall.xlcSelectEnd, (int)Direction.Down);
			int value = ((ExcelReference)XlCall.Excel(XlCall.xlfSelection)).RowLast;
			if (value == My.MaxRowIndex) {
				if (cellValue == ExcelEmpty.Value) {
					lastRowIndex = null;
					return false;
				} else {
					lastRowIndex = cell.RowFirst;
					return true;
				}
			} else {
				lastRowIndex = value;
				return true;
			}
		}
	}
}
