using System.Diagnostics;
using System.Threading.Tasks;

namespace Sample.Daemon {
	public class Program {
		public static Task Main(string[] args) {
			Albatross.Logging.Extensions.RemoveLegacySlackSinkOptions();
			System.Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);
			return new MySetup(args)
				.ConfigureServiceHost<MyHostedService>()
				.RunAsService()
				.RunAsync();
		}
	}
}
