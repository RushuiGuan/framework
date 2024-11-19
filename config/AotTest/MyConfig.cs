using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace AotTest {
	public class MyConfig : ConfigBase {
		public override string Key => "my";
		public MyConfig(IConfiguration configuration) : base(configuration) { }
		public string Name { get; set; } = string.Empty;
	}
}