using System.Diagnostics;
using System.Threading.Tasks;

namespace SampleProject.Daemon {
	public class Program {
		public static Task Main(string[] args) {
			System.Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);
			return new MySetup(args)
				.ConfigureServiceHost<MyHostedService>()
				.RunAsService()
				.RunAsync();
		}
	}
}
