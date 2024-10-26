using Albatross.Excel.SampleAddIn.Models;
using Albatross.Excel.Table;
using ExcelDna.Integration;
using Humanizer;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using interop = Microsoft.Office.Interop.Excel;

namespace Albatross.Excel.SampleAddIn {
	public class TableWriteDemo {
		const string MySheetName = "test-data";
		private readonly ILogger<TableWriteDemo> logger;
		private TableOptions testDataOptions;

		public TableWriteDemo(ILogger<TableWriteDemo> logger) {
			this.logger = logger;
			this.testDataOptions = new TableOptionsBuilder()
				.AddColumns<TestData>()
				.SetCurrent(nameof(TestData.Number), x => x.NumberFormat.StandardNumber())
				.SetCurrent(nameof(TestData.DateTime1), x => x.NumberFormat.StandardDateTime())
				.SetCurrent(nameof(TestData.DateTime2), x => x.NumberFormat.StandardDateTime())
				.SetCurrent(nameof(TestData.Text), x => {
					x.Background.Color(Color.Blue);
					x.FontProperties.Color(Color.White);
				})
				.Add("Diff", typeof(double)).SetFormula("=R[0]C[-1] - R[0]C[-2]")
				.Build();
		}


		async Task<TestData[]> LoadTestData() {
			await Task.Delay(100);
			var random = new Random();
			var max = 1000;
			var result = new TestData[max];
			for (int i = 0; i < max; i++) {
				var item = new TestData {
					Id = i,
					Number = random.Next(),
					DateOnly = new DateTime(random.NextInt64(new DateTime(1990, 1, 1).Ticks, DateTime.MaxValue.Ticks)),
					DateTime1 = new DateTime(random.NextInt64(new DateTime(1990, 1, 1).Ticks, DateTime.MaxValue.Ticks)),
					DateTime2 = new DateTime(random.NextInt64(new DateTime(1990, 1, 1).Ticks, DateTime.MaxValue.Ticks)),
					Text = new DateTime(random.NextInt64(new DateTime(1990, 1, 1).Ticks, DateTime.MaxValue.Ticks)).Humanize(),
				};
				result[i] = item;
			}
			return result;
		}
		public async Task WriteToExcel() {
			interop.Worksheet sheet = My.ActiveWorkbook(MySheetName).GetOrCreateSheet(MySheetName);
			var items = await LoadTestData();
			ExcelAsyncUtil.QueueAsMacro(new ExcelAction(() => items.WriteTable(sheet, testDataOptions)));
		}
	}
}