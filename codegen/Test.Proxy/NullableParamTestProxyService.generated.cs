using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class NullableParamTestProxyService : ClientBase {
		public NullableParamTestProxyService(ILogger<NullableParamTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/nullable-param-test";
		public Task<System.String> NullableStringParam(System.String? text) {
			string path = $"{ControllerPath}/nullable-string-param";
			var queryString = new NameValueCollection();
			if (text != null) {
				queryString.Add("text", text);
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> NullableValueType(System.Nullable<System.Int32> id) {
			string path = $"{ControllerPath}/nullable-value-type";
			var queryString = new NameValueCollection();
			if (id != null) {
				queryString.Add("id", $"{id}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> NullableDateOnly(System.Nullable<System.DateOnly> date) {
			string path = $"{ControllerPath}/nullable-date-only";
			var queryString = new NameValueCollection();
			if (date != null) {
				queryString.Add("date", date.Value.ISO8601String());
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task NullablePostParam(Test.Dto.MyDto? dto) {
			string path = $"{ControllerPath}/nullable-post-param";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.MyDto?>(HttpMethod.Post, path, queryString, dto)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> NullableStringArray(System.String?[] values) {
			string path = $"{ControllerPath}/nullable-string-array";
			var queryString = new NameValueCollection();
			foreach (var item in values) {
				if (item != null) {
					queryString.Add("values", item);
				}
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> NullableStringCollection(System.Collections.Generic.IEnumerable<System.String> values) {
			string path = $"{ControllerPath}/nullable-string-collection";
			var queryString = new NameValueCollection();
			foreach (var item in values) {
				if (item != null) {
					queryString.Add("values", item);
				}
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> NullableValueTypeArray(System.Nullable<System.Int32>[] values) {
			string path = $"{ControllerPath}/nullable-value-type-array";
			var queryString = new NameValueCollection();
			foreach (var item in values) {
				if (item != null) {
					queryString.Add("values", $"{item}");
				}
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> NullableValueTypeCollection(System.Collections.Generic.IEnumerable<System.Nullable<System.Int32>> values) {
			string path = $"{ControllerPath}/nullable-value-type-collection";
			var queryString = new NameValueCollection();
			foreach (var item in values) {
				if (item != null) {
					queryString.Add("values", $"{item}");
				}
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> NullableDateOnlyCollection(System.Collections.Generic.IEnumerable<System.Nullable<System.DateOnly>> dates) {
			string path = $"{ControllerPath}/nullable-date-only-collection";
			var queryString = new NameValueCollection();
			foreach (var item in dates) {
				if (item != null) {
					queryString.Add("dates", item.Value.ISO8601String());
				}
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.String> NullableDateOnlyArray(System.Nullable<System.DateOnly>[] dates) {
			string path = $"{ControllerPath}/nullable-date-only-array";
			var queryString = new NameValueCollection();
			foreach (var item in dates) {
				if (item != null) {
					queryString.Add("dates", item.Value.ISO8601String());
				}
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable
