using Albatross.Hosting;
using System.Threading.Tasks;

namespace Sample.Logging.WebApi {
	public class Program {
		public static Task Main(string[] args) {
			Albatross.Logging.Extensions.RemoveLegacySlackSinkOptions();
			return new Setup(args)
				.ConfigureWebHost<Startup>()
				.RunAsync(args);
		}
	}
}
