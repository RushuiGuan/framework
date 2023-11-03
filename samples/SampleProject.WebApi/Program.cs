using Albatross.Hosting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SampleProject.WebApi {
	public class Program {
		public static Task Main(string[] args) {
			Albatross.Logging.Extensions.RemoveLegacySlackSinkOptions();
			System.Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);
			return new Setup(args)
				.ConfigureWebHost<Startup>()
				.RunAsync(args);
		}
	}
}
