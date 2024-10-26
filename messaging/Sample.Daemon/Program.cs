using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sample.Daemon {
	public class Program {
		public static Task Main(string[] args) {
			Albatross.Logging.Extensions.RemoveLegacySlackSinkOptions();
			System.Environment.CurrentDirectory = AppContext.BaseDirectory;
			return new MySetup(args)
				.ConfigureServiceHost<MyHostedService>()
				.RunAsService()
				.RunAsync();
		}
	}
}