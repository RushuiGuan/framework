using Albatross.Dates;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.WithInterface.Proxy {
	public partial interface IFromRouteParamTestProxyService {
		Task ImplicitRoute(System.String name, System.Int32 id);
		Task ExplicitRoute(System.String name, System.Int32 id);
		Task WildCardRouteDouble(System.String name, System.Int32 id);
		Task WildCardRouteSingle(System.String name, System.Int32 id);
		Task DateTimeRoute(System.DateTime date, System.Int32 id);
		Task DateTimeAsDateOnlyRoute(System.DateTime date, System.Int32 id);
		Task DateOnlyRoute(System.DateOnly date, System.Int32 id);
		Task DateTimeOffsetRoute(System.DateTimeOffset date, System.Int32 id);
		Task TimeOnlyRoute(System.TimeOnly time, System.Int32 id);
	}

	public partial class FromRouteParamTestProxyService : ClientBase, IFromRouteParamTestProxyService {
		public FromRouteParamTestProxyService(ILogger<FromRouteParamTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/from-routing-param-test";
		public async Task ImplicitRoute(System.String name, System.Int32 id) {
			string path = $"{ControllerPath}/implicit-route/{name}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task ExplicitRoute(System.String name, System.Int32 id) {
			string path = $"{ControllerPath}/explicit-route/{name}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task WildCardRouteDouble(System.String name, System.Int32 id) {
			string path = $"{ControllerPath}/wild-card-route-double/{id}/{name}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task WildCardRouteSingle(System.String name, System.Int32 id) {
			string path = $"{ControllerPath}/wild-card-route-single/{id}/{name}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task DateTimeRoute(System.DateTime date, System.Int32 id) {
			string path = $"{ControllerPath}/date-time-route/{date.ISO8601String()}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task DateTimeAsDateOnlyRoute(System.DateTime date, System.Int32 id) {
			string path = $"{ControllerPath}/date-time-as-date-only-route/{date.ISO8601StringDateOnly()}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task DateOnlyRoute(System.DateOnly date, System.Int32 id) {
			string path = $"{ControllerPath}/date-only-route/{date.ISO8601String()}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task DateTimeOffsetRoute(System.DateTimeOffset date, System.Int32 id) {
			string path = $"{ControllerPath}/datetimeoffset-route/{date.ISO8601String()}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task TimeOnlyRoute(System.TimeOnly time, System.Int32 id) {
			string path = $"{ControllerPath}/timeonly-route/{time.ISO8601String()}/{id}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

