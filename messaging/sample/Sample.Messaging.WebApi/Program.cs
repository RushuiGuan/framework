using Albatross.Hosting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sample.Messaging.WebApi {
	public class Program {
		public static async Task Main(string[] args) {
			Albatross.Logging.Extensions.RemoveLegacySlackSinkOptions();
			System.Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);
			await new Setup(args)
				.ConfigureWebHost<Startup>()
				.RunAsync(args);

			System.Console.WriteLine("Exiting program");
		}
	}
}
