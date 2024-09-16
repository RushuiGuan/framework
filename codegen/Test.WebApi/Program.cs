using Albatross.Hosting;
using System.Threading.Tasks;

namespace Test.WebApi {
	internal class Program {
		public static Task Main(string[] args) {
			return new Setup(args)
				.ConfigureWebHost<MyStartup>()
				.RunAsync();
		}
	}
}
