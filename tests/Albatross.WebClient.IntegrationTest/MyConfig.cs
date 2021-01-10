using Albatross.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.WebClient.IntegrationTest {
	public class MyConfig : ConfigBase {
		public string TestUrl { get; private set; }
		public override void Init(IConfiguration configuration) {
			this.TestUrl = configuration.GetEndPoint("test");
		}
	}
	public class GetMyConfig : GetConfig<MyConfig> {
		public GetMyConfig(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => string.Empty;
	}
}
