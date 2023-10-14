using interop = Microsoft.Office.Interop.Excel;
using Albatross.Excel.Sample.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExcelDna.Integration;
using Albatross.Excel.Table;
using System.Linq;
using Albatross.Authentication.Core;
using System;
using Albatross.Collections;

namespace Albatross.Excel.Sample {
	public class TableReadWriteDemo {
		const string MySheetName = "instrument-price";
		private readonly ILogger<TableReadWriteDemo> logger;
		private readonly InstrumentService instrumentService;
		private readonly IGetCurrentUser getCurrentUser;
		private TableOptions priceTableOptions;

		public TableReadWriteDemo(ILogger<TableReadWriteDemo> logger, InstrumentService instrumentService, IGetCurrentUser getCurrentUser) {
			this.logger = logger;
			this.instrumentService = instrumentService;
			this.getCurrentUser = getCurrentUser;
			this.priceTableOptions = new TableOptionsBuilder()
				.AddColumns<PriceViewModel>()
				.Add("Instrument", typeof(string)).SetFormula("=InstrumentName(<InstrumentId>)").Order(2.5f)
				.SetCurrent("Date").SetNumberFormat(x => x.StandardDate()).SetFormula("=Today()")
				.AddTemplate(x => x.NumberFormat.StandardNumber(4)).AddTemplate(x => x.Required())
				.ApplyTemplate("Open", "High", "Low", "Close")
				.ClearTemplate()
				.AddTemplate(x => {
					x.Background.Color(Color.LightGray);
					x.ReadOnly = true;
				})
				.ApplyTemplate("Id").Order(100)
				.ApplyTemplate("CreatedBy").Order(102)
				.ApplyTemplate("CreatedUtc").Order(103).SetNumberFormat(x => x.StandardDateTime())
				.Build();
		}

		public async Task<IEnumerable<PriceViewModel>> LoadPriceViewModels() {
			var instruments = await instrumentService.GetInstruments();
			return instruments.Select(args => new PriceViewModel(args)).ToArray();
		}
		async Task<IEnumerable<Price>> SavePrice(IEnumerable<Price> data) {
			await Task.Delay(100);
			var counter = new Random().Next();
			foreach (var item in data) {
				item.Id = counter++;
				item.CreatedBy = this.getCurrentUser.Get();
				item.CreatedUtc = System.DateTime.UtcNow;
			}
			return data;
		}

		public async void LoadPriceData() {
			interop.Worksheet sheet = My.ActiveWorkbook(MySheetName).GetOrCreateSheet(MySheetName);
			var items = await LoadPriceViewModels();
			ExcelAsyncUtil.QueueAsMacro(new ExcelAction(() => items.ToArray().WriteTable(sheet, priceTableOptions)));
		}

		public void SavePrice() {
			var book = My.ActiveWorkbook();
			if (book.TryGetSheet(MySheetName, out var sheet)) {
				ExcelAsyncUtil.QueueAsMacro(() => {
					var result = sheet.ReadTable<PriceViewModel>(this.priceTableOptions);
					if (result.Errors.Any()) {
						logger.LogError("Error getting price from excel\n{@error}", result.Errors);
					}
					if (result.Data.Any()) {
						ExcelAsyncUtil.QueueAsMacro(async () => {
							var prices = new List<Price>();
							foreach (var vm in result.Data) {
								if (vm.TryCreatePrice(out var price)) {
									prices.Add(price);
								} else {
									logger.LogError("Error creating price from viewmodel {@vm}", vm);
								}
							}
							var saveResult = await SavePrice(prices);
							result.Data.Merge(saveResult, src => new { src.InstrumentId, src.Date }, dst => new { dst.InstrumentId, dst.Date },
								(src, dst) => dst.Set(src), src => {
									logger.LogError("Could not find a match for received price save result {@result}", src);
								}, dst => { });
							ExcelAsyncUtil.QueueAsMacro(new ExcelAction(() => result.Data.ToArray().UpdateTable(sheet, priceTableOptions, true)));
						});
					}
				});
			}
		}
	}
}
