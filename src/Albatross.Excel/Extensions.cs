using ExcelDna.Integration;
using System;

namespace Albatross.Excel {
	public static class Extensions {
		public static CellBuilder FontProperties(this CellBuilder builder, Action<FontProperties> action)
			=> builder.Use(action);
		public static CellBuilder NumberFormat(this CellBuilder builder, Action<NumberFormat> action)
			=> builder.Use(action);
		public static CellBuilder Background(this CellBuilder builder, Action<Background> action)
			=> builder.Use(action);

		public static bool IsSingleCell(this ExcelReference range) => range.ColumnFirst == range.ColumnLast && range.RowFirst == range.RowLast;
		public static void Clear(this ExcelReference range) {
			XlCall.Excel(XlCall.xlcSelect, range);
			XlCall.Excel(XlCall.xlcClear);
		}
	}
}
