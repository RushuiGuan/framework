using Albatross.Hosting;
using System.Threading.Tasks;

namespace Albatross.WebClient.TestApi {
	public class Program {
		public static Task Main(string[] args) {
			return new Setup(args).ConfigureWebHost<TestApiStartup>().RunAsync();
		}
	}
}
