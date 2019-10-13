using Microsoft.AspNetCore.Hosting;

namespace Albatross.Host.AspNetCore.UnitTest {
    public class Program {
		public static void Main(string[] args) {
			new Albatross.Host.AspNetCore.KestrelWebHost<Startup>().Run();
        }
	}
}