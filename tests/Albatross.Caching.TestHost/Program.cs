using Albatross.Hosting;
using System.Threading.Tasks;

namespace Albatross.Caching.TestHost {
	public class Program {
		public static Task Main(string[] args) {
			return new Setup(args)
				.ConfigureWebHost<MyStartup>()
				.RunAsync(args);
		}
	}
}