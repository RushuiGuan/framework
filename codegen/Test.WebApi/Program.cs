using Albatross.Hosting;
using System.Threading.Tasks;

namespace Test.WebApi {
	internal class Program {
		public static Task Main(string[] args) {
			System.Environment.CurrentDirectory = System.AppContext.BaseDirectory;
			Albatross.Logging.Extensions.RemoveLegacySlackSinkOptions();
			return new Setup(args)
				.ConfigureWebHost<MyStartup>()
				.RunAsync();
		}
	}
}