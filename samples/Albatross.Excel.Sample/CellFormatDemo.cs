using Albatross.Excel.Table;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Excel.Sample {
	public class CellFormatDemo {
		const string SampleSheetName = "sample";

		private readonly ILogger<CellFormatDemo> logger;
		private TableOptions printColorOptions;
		private TableOptions fontProperyTestCaseOptions;
		private TableOptions numberFormatTestCaseOptions;
		private TableOptions backgroundTestCaseOptions;

		public CellFormatDemo(ILogger<CellFormatDemo> logger) {
			this.logger = logger;
			this.printColorOptions = new TableOptionsBuilder()
				.AddColumns(
					new TableColumn("Color", typeof(string)) {
						ReadEntityPropertyHandler = (column, args) => args,
					})
				.Build();
			this.fontProperyTestCaseOptions = new TableOptionsBuilder()
				.Offset(0, 1)
				.AddColumns(
					new TableColumn("FontProperties", typeof(string)) {
						ReadEntityPropertyHandler = (column, args) => args,
					})
				.Build();
			this.numberFormatTestCaseOptions = new TableOptionsBuilder()
				.Offset(0, 2)
				.AddColumns(
					new TableColumn("NumberFormats", typeof(double)) {
						ReadEntityPropertyHandler = (column, args) => args,
					})
				.Build();
			this.backgroundTestCaseOptions = new TableOptionsBuilder()
				.Offset(0, 3)
				.AddColumns(
					new TableColumn("Background", typeof(string)) {
						ReadEntityPropertyHandler = (column, args) => args,
					})
				.Build();
		}
		public void PrintColorDemo() {
			var sheet = My.ActiveWorkbook(SampleSheetName).GetOrCreateSheet(SampleSheetName);
			ExcelAsyncUtil.QueueAsMacro(new ExcelAction(() => {
				var range = Enum.GetNames<Color>().WriteTable(sheet, printColorOptions);
				for (int i = 1; i <= range.RowLast; i++) {
					new CellBuilder(i, range.ColumnFirst)
						.FontProperties(args => args.Bold().Color((Color)i - 1))
						.Apply();
				}
			}));
		}
		string[] fontPropertiesTestCases = new string[] {
			"Font - Arial",
			"Font - Calibri",
			"Bold",
			"Italic",
			"BoldItalic",
			"Regular",
			"Size 6",
			"Size 12",
			"Superscript",
			"Subscript",
			"Strikethrough",
			"SingleUnderline",
			"DoubleUnderline",
			"SingleAccountingUnderline",
			"DoubleAccountingUnderline",
			"NoUnderline",
		};
		public void FontPropertiesDemo() {
			var sheet = My.ActiveWorkbook().GetOrCreateSheet(SampleSheetName);
			ExcelAsyncUtil.QueueAsMacro(new ExcelAction(() => {
				var range = fontPropertiesTestCases.WriteTable(sheet, fontProperyTestCaseOptions);
				new CellBuilder(range.RowFirst + 1, range.ColumnFirst).FontProperties(x => x.Font("Arial")).Apply();
				new CellBuilder(range.RowFirst + 2, range.ColumnFirst).FontProperties(x => x.Font("Calibri")).Apply();
				new CellBuilder(range.RowFirst + 3, range.ColumnFirst).FontProperties(x => x.Bold()).Apply();
				new CellBuilder(range.RowFirst + 4, range.ColumnFirst).FontProperties(x => x.Italic()).Apply();
				new CellBuilder(range.RowFirst + 5, range.ColumnFirst).FontProperties(x => x.BoldItalic()).Apply();
				new CellBuilder(range.RowFirst + 6, range.ColumnFirst).FontProperties(x => x.Regular()).Apply();

				new CellBuilder(range.RowFirst + 7, range.ColumnFirst).FontProperties(x => x.Size(6)).Apply();
				new CellBuilder(range.RowFirst + 8, range.ColumnFirst).FontProperties(x => x.Size(12)).Apply();
				new CellBuilder(range.RowFirst + 9, range.ColumnFirst).FontProperties(x => x.Superscript()).Apply();
				new CellBuilder(range.RowFirst + 10, range.ColumnFirst).FontProperties(x => x.Subscript()).Apply();
				new CellBuilder(range.RowFirst + 11, range.ColumnFirst).FontProperties(x => x.Strikethrough()).Apply();
				new CellBuilder(range.RowFirst + 12, range.ColumnFirst).FontProperties(x => x.SingleUnderline()).Apply();
				new CellBuilder(range.RowFirst + 13, range.ColumnFirst).FontProperties(x => x.DoubleUnderline()).Apply();
				new CellBuilder(range.RowFirst + 14, range.ColumnFirst).FontProperties(x => x.SingleAccountingUnderline()).Apply();
				new CellBuilder(range.RowFirst + 15, range.ColumnFirst).FontProperties(x => x.DoubleAccountingUnderline()).Apply();
				new CellBuilder(range.RowFirst + 16, range.ColumnFirst).FontProperties(x => x.NoUnderline()).Apply();
			}));
		}
		double[] numberFormatTestCases = new double[] {
			1000,		// 1,000
			0.66451,	// 66%
			0.66451,	// 66.45%
			9999999,	// DeleteFormat
			44927.00,	// 2023-01-01
			45291.5833333,	// 2023-01-01 02:00:00 PM
		};
		public void NumberFormatsDemo() {
			var sheet = My.ActiveWorkbook(SampleSheetName).GetOrCreateSheet(SampleSheetName);
			ExcelAsyncUtil.QueueAsMacro(new ExcelAction(() => {
				var range = numberFormatTestCases.WriteTable(sheet, numberFormatTestCaseOptions);
				new CellBuilder(range.RowFirst + 1, range.ColumnFirst).NumberFormat(x => x.StandardNumber()).Apply();
				new CellBuilder(range.RowFirst + 2, range.ColumnFirst).NumberFormat(x => x.Percent(0)).Apply();
				new CellBuilder(range.RowFirst + 3, range.ColumnFirst).NumberFormat(x => x.Percent(2)).Apply();
				new CellBuilder(range.RowFirst + 4, range.ColumnFirst).NumberFormat(x => x.DeleteFormat()).Apply();
				new CellBuilder(range.RowFirst + 5, range.ColumnFirst).NumberFormat(x => x.StandardDate()).Apply();
				new CellBuilder(range.RowFirst + 6, range.ColumnFirst).NumberFormat(x => x.StandardDateTime()).Apply();
			}));
		}
		string[] backgroundTestCases = new string[] {
			"Solid_Yellow",
			"Solid_Blue",
			"Solid_Red",
			"Pattern9_Red",
			"NoPattern_Cyan"
		};
		public void BackgroundDemo() {
			var sheet = My.ActiveWorkbook(SampleSheetName).GetOrCreateSheet(SampleSheetName);
			ExcelAsyncUtil.QueueAsMacro(new ExcelAction(() => {
				var range = backgroundTestCases.WriteTable(sheet, backgroundTestCaseOptions);
				new CellBuilder(range.RowFirst + 1, range.ColumnFirst).Background(x => x.Solid().Color(Color.Yellow)).Apply();
				new CellBuilder(range.RowFirst + 2, range.ColumnFirst).Background(x => x.Solid().Color(Color.Blue)).Apply();
				new CellBuilder(range.RowFirst + 3, range.ColumnFirst).Background(x => x.Solid().Color(Color.Red)).Apply();
				new CellBuilder(range.RowFirst + 4, range.ColumnFirst).Background(x => x.Pattern(Pattern.Pattern9).Color(Color.Red)).Apply();
				new CellBuilder(range.RowFirst + 5, range.ColumnFirst).Background(x => x.Color(Color.Cyan)).Apply();
			}));
		}
	}
}
