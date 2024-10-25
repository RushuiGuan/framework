using Albatross.Dates;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class RequiredParamTestProxyService : ClientBase {
		public RequiredParamTestProxyService(ILogger<RequiredParamTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/required-param-test";
		public async Task<System.String> ExplicitStringParam(System.String text) {
			string path = $"{ControllerPath}/explicit-string-param";
			var queryString = new NameValueCollection();
			queryString.Add("text", $"{text}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> ImplicitStringParam(System.String text) {
			string path = $"{ControllerPath}/implicit-string-param";
			var queryString = new NameValueCollection();
			queryString.Add("text", $"{text}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredStringParam(System.String text) {
			string path = $"{ControllerPath}/required-string-param";
			var queryString = new NameValueCollection();
			queryString.Add("text", $"{text}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredValueType(System.Int32 id) {
			string path = $"{ControllerPath}/required-value-type";
			var queryString = new NameValueCollection();
			queryString.Add("id", $"{id}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredDateOnly(System.DateOnly date) {
			string path = $"{ControllerPath}/required-date-only";
			var queryString = new NameValueCollection();
			queryString.Add("date", $"{date:yyyy-MM-dd}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredDateTime(System.DateTime date) {
			string path = $"{ControllerPath}/required-datetime";
			var queryString = new NameValueCollection();
			queryString.Add("date", $"{date:yyyy-MM-ddTHH:mm:ssK}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredDateTimeAsDateOnly(System.DateTime date) {
			string path = $"{ControllerPath}/requried-datetime-as-dateonly";
			var queryString = new NameValueCollection();
			queryString.Add("date", $"{date:yyyy-MM-dd}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task RequiredPostParam(Test.Dto.Classes.MyDto dto) {
			string path = $"{ControllerPath}/required-post-param";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.Classes.MyDto>(HttpMethod.Post, path, queryString, dto)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredStringArray(System.String[] values) {
			string path = $"{ControllerPath}/required-string-array";
			var queryString = new NameValueCollection();
			foreach (var item in values) {
				queryString.Add("values", $"{item}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredStringCollection(System.Collections.Generic.IEnumerable<System.String> values) {
			string path = $"{ControllerPath}/required-string-collection";
			var queryString = new NameValueCollection();
			foreach (var item in values) {
				queryString.Add("values", $"{item}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredValueTypeArray(System.Int32[] values) {
			string path = $"{ControllerPath}/required-value-type-array";
			var queryString = new NameValueCollection();
			foreach (var item in values) {
				queryString.Add("values", $"{item}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredValueTypeCollection(System.Collections.Generic.IEnumerable<System.Int32> values) {
			string path = $"{ControllerPath}/required-value-type-collection";
			var queryString = new NameValueCollection();
			foreach (var item in values) {
				queryString.Add("values", $"{item}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredDateOnlyCollection(System.Collections.Generic.IEnumerable<System.DateOnly> dates) {
			string path = $"{ControllerPath}/required-date-only-collection";
			var queryString = new NameValueCollection();
			foreach (var item in dates) {
				queryString.Add("dates", $"{item:yyyy-MM-dd}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredDateOnlyArray(System.DateOnly[] dates) {
			string path = $"{ControllerPath}/required-date-only-array";
			var queryString = new NameValueCollection();
			foreach (var item in dates) {
				queryString.Add("dates", $"{item:yyyy-MM-dd}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredDateTimeCollection(System.Collections.Generic.IEnumerable<System.DateTime> dates) {
			string path = $"{ControllerPath}/required-datetime-collection";
			var queryString = new NameValueCollection();
			foreach (var item in dates) {
				queryString.Add("dates", $"{item:yyyy-MM-ddTHH:mm:ssK}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredDateTimeArray(System.DateTime[] dates) {
			string path = $"{ControllerPath}/required-datetime-array";
			var queryString = new NameValueCollection();
			foreach (var item in dates) {
				queryString.Add("dates", $"{item:yyyy-MM-ddTHH:mm:ssK}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredDateTimeAsDateOnlyCollection(System.Collections.Generic.IEnumerable<System.DateTime> dates) {
			string path = $"{ControllerPath}/required-datetime-as-dateonly-collection";
			var queryString = new NameValueCollection();
			foreach (var item in dates) {
				queryString.Add("dates", $"{item:yyyy-MM-dd}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}

		public async Task<System.String> RequiredDateTimeAsDateOnlyArray(System.DateTime[] dates) {
			string path = $"{ControllerPath}/required-datetime-as-dateonly-array";
			var queryString = new NameValueCollection();
			foreach (var item in dates) {
				queryString.Add("dates", $"{item:yyyy-MM-dd}");
			}

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

