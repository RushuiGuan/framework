using Albatross.Excel.SampleAddIn.Models;
using ExcelDna.Integration;
using ExcelDna.Registration.Utils;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Excel.SampleAddIn {
	public class InstrumentService {
		Dictionary<int, Instrument> instruments;
		private readonly ILogger<InstrumentService> logger;

		public InstrumentService(ILogger<InstrumentService> logger) {
			instruments = new Instrument[] {
				new Instrument("apple", "a"){ Id = 1 },
				new Instrument("microsoft", "m"){ Id = 2 },
				new Instrument("tesla", "t"){ Id = 3 }
			}.ToDictionary(x => x.Id, x => x);
			this.logger = logger;
		}

		[ExcelFunction]
		public async Task<object> Instruments() {
			var support = new FunctionSupport();
			try {
				await Task.Delay(100);
				return instruments.Values.ToArray().CreateRangeValueByReflection();
			} finally {
				support.Queue(caller => new CellBuilder(caller.RowFirst, caller.RowFirst, caller.ColumnFirst, caller.ColumnFirst + 2)
					.FontProperties(x => x.Bold()).Apply())
					.RestoreSelectionToCaller().Execute();
			}
		}

		[ExcelFunction]
		public async Task<object> Instruments2() {
			await Task.Delay(100);
			return instruments.Values.CreateRangeValue(new string[] {"id", "name", "ticker" }, x => x.Id, x => x.Name, x => x.Ticker);
		}

		[ExcelFunction(IsMacroType = true)]
		public object InstrumentName([ExcelArgument(Description = "Instrument Id")] object idCell) {
			var functionName = nameof(InstrumentId);
			var parameters = new object[] { idCell, };
			logger.LogInformation("Calling {name} w. {@param}", functionName, parameters);
			return AsyncTaskUtil.RunTask<object>(functionName, parameters, async () => {
				await Task.Delay(100);
				if(CellValue.TryReadInteger(idCell, out var id)) {
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
