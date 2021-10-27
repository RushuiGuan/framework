using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public class CachingClient : ClientBase {
		public CachingClient(ILogger @logger, HttpClient @client) : base(@logger, @client) { }
		public const string ControllerPath = "api/caching";
		public const string CachingProxyName = "CachingProxy";

		public async Task Envict(IEnumerable<object> @keys) {
			string path = $"{ControllerPath}/envict";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateJsonRequest<IEnumerable<string?>>(HttpMethod.Post, path, queryString, @keys.Select(args => Convert.ToString(args)))) {
				await this.Invoke(request);
			}
		}

		public async Task Reset() {
			string path = $"{ControllerPath}";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.Invoke(request);
			}
		}
	}
}