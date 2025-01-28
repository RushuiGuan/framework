using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Test.Proxy {
	public partial class OmittedConstructorProxyService {
		public OmittedConstructorProxyService(ILogger<OmittedConstructorProxyService> logger, HttpClient client) : base(logger, client) {
		}
	}
}

