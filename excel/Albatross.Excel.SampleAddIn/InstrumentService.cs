using Albatross.Excel.SampleAddIn.Models;
using ExcelDna.Integration;
using ExcelDna.Registration.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Excel.SampleAddIn {
	public class InstrumentService {
		Dictionary<int, Instrument> instruments;
		private readonly ILogger<InstrumentService> logger;

		public InstrumentService(ILogger<InstrumentService> logger) {
			instruments = new Instrument[] {
				new Instrument("apple", "a"){ Id = 8888, Number = 9999, Date = DateTime.Today, },
				new Instrument("microsoft", "m"){ Id = 8887, Number = 9998 },
				new Instrument("tesla", "t"){ Id = 8884, Number = 9997 },
				new Instrument("tesla", "t"){ Id = 8881, Number = 9996 }
			}.ToDictionary(x => x.Id, x => x);
			this.logger = logger;
		}

		[ExcelFunction(Description = "test me")]
		public async Task<object> Instruments() {
			var builder = new ArrayFunctionBuilder(typeof(Instrument), true)
				.AddColumnsByReflection().SetOrder("Id", "Name", "Date")
				.DefaultFormatHeader().FormatColumns(builder => builder.NumberFormat(x => x.StandardDate()), "Date").RestoreSelection();
			await Task.Delay(100);
			return builder.SetValue(instruments.Values);
		}
		[ExcelFunction(HelpTopic = "test you")]
		public async Task<object> Instruments2() {
			var builder = new ArrayFunctionBuilder(typeof(Instrument), false).AddColumnsByReflection()
				.DefaultFormatHeader().FormatColumns(builder => builder.NumberFormat(x => x.StandardDate()), "Date")
				.FormatColumns(builder => builder.NumberFormat(x => x.StandardNumber()), "Id", "Number")
				.BoldHeader().RestoreSelection().SetOrder("Id");
			await Task.Delay(100);
			return builder.SetValue(instruments.Values);
		}


		[ExcelFunction(IsMacroType = true)]
		public object InstrumentName([ExcelArgument(Description = "Instrument Id")] object idCell) {
			var functionName = nameof(InstrumentId);
			var parameters = new object[] { idCell, };
			logger.LogInformation("Calling {name} w. {@param}", functionName, parameters);
			return AsyncTaskUtil.RunTask<object>(functionName, parameters, async () => {
				await Task.Delay(100);
				if (CellValue.TryReadInteger(idCell, out var id)) {
					if (instruments.TryGetValue(id, out Instrument instrument)) {
						return instrument.Name;
					}
				}
				return ExcelError.ExcelErrorValue;
			});
		}

		[ExcelFunction]
		public async Task<object> InstrumentId([ExcelArgument(Description = "Instrument Name")] object nameCell) {
			logger.LogInformation("Calling {name} w. {@param}", nameof(InstrumentId), nameCell);
			await Task.Delay(100);
			if (CellValue.TryReadString(nameCell, out var name)) {
				var item = instruments.Values.Where(args => string.Equals(args.Name, name, System.StringComparison.InvariantCultureIgnoreCase))
				.FirstOrDefault();
				if (item != null) {
					return item.Id;
				}
			}
			return ExcelError.ExcelErrorValue;
		}

		public async Task<IEnumerable<Instrument>> GetInstruments() {
			// use delay here to create an async method since in real life, this will most likely be a external db or api call
			await Task.Delay(100);
			return instruments.Values;
		}
	}
}