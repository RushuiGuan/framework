using Albatross.Hosting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sample.WebApi {
	public class Program {
		public static async Task Main(string[] args) {
			Albatross.Logging.Extensions.RemoveLegacySlackSinkOptions();
			System.Environment.CurrentDirectory = AppContext.BaseDirectory;
			await new Setup(args)
				.ConfigureWebHost<Startup>()
				.RunAsync();

			System.Console.WriteLine("Exiting program");
		}
	}
}