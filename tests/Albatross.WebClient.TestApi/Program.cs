using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace Albatross.WebClient.TestApi {
    public class Program {
		public static async Task Main(string[] args) {
			await new Albatross.Host.AspNetCore.WebHost<TestApiStartup>().RunAsync(args);
		}
	}
}
