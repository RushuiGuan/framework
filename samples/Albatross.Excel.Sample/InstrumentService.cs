using Albatross.Excel.Sample.Models;
using ExcelDna.Integration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Excel.Sample {
	public class InstrumentService {
		Dictionary<int, Instrument> instruments;
		public InstrumentService() {
			instruments = new Instrument[] {
				new Instrument("apple", "a"){ Id = 1 },
				new Instrument("microsoft", "m"){ Id = 2 },
				new Instrument("tesla", "t"){ Id = 3 }
			}.ToDictionary(x=>x.Id, x => x);
		}

		[ExcelFunction]
		public object InstrumentName([ExcelArgument(Description ="Instrument Id")]int id) {
			if(instruments.TryGetValue(id, out Instrument value)) {
				return value.Name;
			} else {
				return ExcelError.ExcelErrorValue;
			}
		}

		public async Task<IEnumerable<Instrument>> GetInstruments() {
			// use delay here to create an async method since in real life, this will most likely be a external db or api call
			await Task.Delay(100);
			return instruments.Values;
		}
	}
}
