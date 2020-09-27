using System.Threading.Tasks;

namespace Albatross.Hosting.Test{
	public class Program {
		public static Task Main(string[] args) {
			return new Setup(args)
				.ConfigureServiceHost<MyServiceHost>()
				.RunAsService()
				.ConfigureWebHost<MyStartup>()
				.RunAsync();
        }
	}
}