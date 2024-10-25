using Albatross.Dates;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class FromQueryParamTestProxyService : ClientBase {
		public FromQueryParamTestProxyService(ILogger<FromQueryParamTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/from-query-param-test";
		public async Task RequiredString(System.String name) {
			string path = $"{ControllerPath}/required-string";
			var queryString = new NameValueCollection();
			queryString.Add("name", $"{name}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task RequiredStringImplied(System.String name) {
			string path = $"{ControllerPath}/required-string-implied";
			var queryString = new NameValueCollection();
			queryString.Add("name", $"{name}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task RequiredStringDiffName(System.String name) {
			string path = $"{ControllerPath}/required-string-diff-name";
			var queryString = new NameValueCollection();
			queryString.Add("n", $"{name}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task RequiredDateTime(System.DateTime datetime) {
			string path = $"{ControllerPath}/required-datetime";
			var queryString = new NameValueCollection();
			queryString.Add("datetime", $"{datetime:yyyy-MM-ddTHH:mm:ssK}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task RequiredDateTimeDiffName(System.DateTime datetime) {
			string path = $"{ControllerPath}/required-datetime_diff-name";
			var queryString = new NameValueCollection();
			queryString.Add("d", $"{datetime:yyyy-MM-ddTHH:mm:ssK}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task RequiredDateOnly(System.DateOnly dateonly) {
			string path = $"{ControllerPath}/required-dateonly";
			var queryString = new NameValueCollection();
			queryString.Add("dateonly", $"{dateonly:yyyy-MM-dd}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task RequiredDateOnlyDiffName(System.DateOnly dateonly) {
			string path = $"{ControllerPath}/required-dateonly_diff-name";
			var queryString = new NameValueCollection();
			queryString.Add("d", $"{dateonly:yyyy-MM-dd}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task RequiredDateTimeOffset(System.DateTimeOffset dateTimeOffset) {
			string path = $"{ControllerPath}/required-datetimeoffset";
			var queryString = new NameValueCollection();
			queryString.Add("dateTimeOffset", $"{dateTimeOffset:yyyy-MM-ddTHH:mm:ssK}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task RequiredDateTimeOffsetDiffName(System.DateTimeOffset dateTimeOffset) {
			string path = $"{ControllerPath}/required-datetimeoffset_diff-name";
			var queryString = new NameValueCollection();
			queryString.Add("d", $"{dateTimeOffset:yyyy-MM-ddTHH:mm:ssK}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

