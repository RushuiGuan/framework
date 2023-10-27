using ExcelDna.Integration;
using ExcelDna.Registration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Albatross.Excel.SampleAddIn {
	public static class Macro {
		public static ILogger Logger { get; internal set; } = null!;

		[ExcelAsyncFunction]
		public static async Task<object> GetName([ExcelArgument(Description = "Instrument Id")] int idCell) {
			Logger.LogInformation("Called {name} with {@param}", nameof(GetName), idCell);
			await Task.Delay(1000);
			return "apple";
		}



		[ExcelAsyncFunction]
		public static async Task<object> GetId([ExcelArgument(Description = "Instrument Name")] string nameCell) {
			Logger.LogInformation("Called {name} with {@param}", nameof(GetId), nameCell);
			await Task.Delay(1000);
			return 1;
		}



		[ExcelFunction()]
		public static object GetId2([ExcelArgument(Description = "Instrument Name")] object nameCell) {
			Logger.LogInformation("Called {name} with {@param}", nameof(GetId2), nameCell);
			if (nameCell is ExcelError) {
				return nameCell;
			}
			return 1;
		}

		[ExcelFunction()]
		public static object GetArray() {
			object[,] array = new object[10, 1];
			for(int i=0;i< 10;i++) {
				array[i, 0] = i;
			}
			return array;
		}


		[ExcelAsyncFunction]
		[ExcelFunction()]
		public static async Task<object> GetAsyncArray() {
			await Task.Delay(5000);
			object[,] array = new object[10, 10];
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					array[i, j] = i;
				}
			}
			return array;
		}
	}
}
