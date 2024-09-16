using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class QueryStringParamTestProxyService : ClientBase {
		public QueryStringParamTestProxyService(ILogger<QueryStringParamTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/query-string-param-test";
		public Task<System.String> Get(System.String name, System.Int32 id) {
			string path = $"{ControllerPath}";
			var queryString = new NameValueCollection();
			queryString.Add("name", name);
			queryString.Add("id", id.ToString());
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> GetWithNullableParam(System.String? name, System.Nullable<System.Int32> id) {
			string path = $"{ControllerPath}/test-nullable-params";
			var queryString = new NameValueCollection();
			if (name != null) {
				queryString.Add("name", name);
			}

			if (id != null) {
				queryString.Add("id", id.ToString());
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> TestDateTime(System.DateTime dateTime) {
			string path = $"{ControllerPath}/test-datetime";
			var queryString = new NameValueCollection();
			queryString.Add("dateTime", dateTime.ISO8601String());
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> TestDateOnly(System.DateOnly date) {
			string path = $"{ControllerPath}/test-dateOnly";
			var queryString = new NameValueCollection();
			queryString.Add("date", date.ISO8601String());
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> TestDateOnly(System.DateTimeOffset dateTimeOffset) {
			string path = $"{ControllerPath}/test-datetimeoffset";
			var queryString = new NameValueCollection();
			queryString.Add("dateTimeOffset", dateTimeOffset.ISO8601String());
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> TestArrayInput(System.String[] items) {
			string path = $"{ControllerPath}/array-query-string";
			var queryString = new NameValueCollection();
			foreach (var item in items) {
				queryString.Add("items", item);
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> TestIEnumerableGenericInput(System.Collections.Generic.IEnumerable<System.String> items) {
			string path = $"{ControllerPath}/ienumerable-generic-query-string";
			var queryString = new NameValueCollection();
			foreach (var item in items) {
				queryString.Add("items", item);
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> TestIEnumerableInput(System.Collections.IEnumerable items) {
			string path = $"{ControllerPath}/ienumerable-query-string";
			var queryString = new NameValueCollection();
			queryString.Add("items", items.ToString());
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> TestQueryName(System.String[] items) {
			string path = $"{ControllerPath}/query-string-with-diff-name";
			var queryString = new NameValueCollection();
			foreach (var item in items) {
				queryString.Add("i", item);
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

