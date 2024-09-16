using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class RoutingParamTestProxyService : ClientBase {
		public RoutingParamTestProxyService(ILogger<RoutingParamTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/routing-param-test";
		public Task Route1(System.String name, System.Int32 id) {
			string path = $"{ControllerPath}/1/{name}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task Route2(System.String name, System.Int32 id) {
			string path = $"{ControllerPath}/2/{name}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task MultipleRouteTest(System.String name, System.Int32 id) {
			string path = $"{ControllerPath}/3/{name}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task Route4(System.String name, System.Int32 id) {
			string path = $"{ControllerPath}/wild-card-route/{name}";
			var queryString = new NameValueCollection();
			queryString.Add("id", id.ToString());
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task Route5(System.String name, System.Int32 id) {
			string path = $"{ControllerPath}/wild-card-route2/{name}";
			var queryString = new NameValueCollection();
			queryString.Add("id", id.ToString());
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task RouteWithDateTime(System.DateTime date, System.Int32 id) {
			string path = $"{ControllerPath}/route-with-date-time/{date.ISO8601String()}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task RouteWithDateTimeAsDateOnly(System.DateTime date, System.Int32 id) {
			string path = $"{ControllerPath}/route-with-date-time-as-date-only/{date.ISO8601String()}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task RouteWithDateOnly(System.DateOnly date, System.Int32 id) {
			string path = $"{ControllerPath}/route-with-date-only/{date.ISO8601String()}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task RouteWithoutRouteAttribute(System.DateOnly date) {
			string path = $"{ControllerPath}/route-without-attribute/{date.ISO8601String()}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

