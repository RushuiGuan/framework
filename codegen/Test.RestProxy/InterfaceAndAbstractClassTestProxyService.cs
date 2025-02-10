using Albatross.RestClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;

namespace Test.RestProxy {
	public partial interface IInterfaceAndAbstractClassTestProxyService {
		Task SubmitByInterface(Test.Dto.Classes.ICommand command);
	}

	public partial class InterfaceAndAbstractClassTestProxyService : IInterfaceAndAbstractClassTestProxyService {
		public InterfaceAndAbstractClassTestProxyService(ILogger<InterfaceAndAbstractClassTestProxyService> logger, HttpClient client)  {
			this.logger = logger;
			this.client = client;
		}

		public const string ControllerPath = "api/interface-abstract-class-test";
		private readonly ILogger<InterfaceAndAbstractClassTestProxyService> logger;
		private readonly HttpClient client;

		public async Task SubmitByInterface(Test.Dto.Classes.ICommand command) {
			string path = $"{ControllerPath}/interface-as-param";
			var queryString = new NameValueCollection {
				{ "command", $"{command}" }
			};
			var options = new RequestOptions { };
			using (var request = RequestExtensions.CreateRequest(HttpMethod.Post, path, queryString, options)) {
				using (var response = await client.SendAsync(request)) {
				}
			}
		}
	}
}
