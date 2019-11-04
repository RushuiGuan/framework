using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace Albatross.Host.AspNetCore.Test {
    public class Program {
		public static Task Main(string[] args) {
			return new Albatross.Host.AspNetCore.WebHost<Startup>().RunAsync();
        }
	}
}