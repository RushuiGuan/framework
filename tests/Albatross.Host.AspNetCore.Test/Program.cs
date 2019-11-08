using Albatross.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace Albatross.Host.AspNetCore.Test {
    public class Program {
		public static async Task Main(string[] args) {
			await new Albatross.Host.AspNetCore.WebHost<Startup>().RunAsync();
        }
	}
}