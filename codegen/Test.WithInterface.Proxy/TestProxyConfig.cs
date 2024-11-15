using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Test.WithInterface.Proxy {
	public class TestProxyConfig : ConfigBase {
		public TestProxyConfig(IConfiguration configuration) : base(configuration) {
			this.EndPoint = configuration.GetRequiredEndPoint("test");
		}
		public string EndPoint { get; }
	}
}