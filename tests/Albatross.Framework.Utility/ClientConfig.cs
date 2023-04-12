using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Albatross.Framework.Utility {
	public class ClientConfig : ConfigBase {
		public ClientConfig(IConfiguration configuration) : base(configuration) {
			TestApiEndPoint = configuration.GetRequiredEndPoint("test-api");
		}

		public string TestApiEndPoint { get; init; }
	}
}
