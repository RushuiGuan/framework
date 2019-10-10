using Microsoft.AspNetCore.Hosting;

namespace Albatross.Test.AspNetCore {
    public class Program {
		public static void Main(string[] args) {
			new Albatross.Host.AspNetCore.KestrelWebHost<Startup>().Run();
        }
	}
}