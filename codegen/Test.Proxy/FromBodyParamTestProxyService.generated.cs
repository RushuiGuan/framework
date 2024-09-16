using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class FromBodyParamTestProxyService : ClientBase {
		public FromBodyParamTestProxyService(ILogger<FromBodyParamTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/from-body-param-test";
		public Task RequiredObject(Test.Dto.MyDto dto) {
			string path = $"{ControllerPath}/required-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.MyDto>(HttpMethod.Post, path, queryString, dto)) {
				return this.GetRawResponse(request);
			}
		}

		public Task NullableObject(Test.Dto.MyDto? dto) {
			string path = $"{ControllerPath}/nullable-object";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.MyDto?>(HttpMethod.Post, path, queryString, dto)) {
				return this.GetRawResponse(request);
			}
		}

		public Task RequiredInt(System.Int32 value) {
			string path = $"{ControllerPath}/required-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<System.Int32>(HttpMethod.Post, path, queryString, value)) {
				return this.GetRawResponse(request);
			}
		}

		public Task NullableInt(System.Nullable<System.Int32> value) {
			string path = $"{ControllerPath}/nullable-int";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<System.Nullable<System.Int32>>(HttpMethod.Post, path, queryString, value)) {
				return this.GetRawResponse(request);
			}
		}

		public Task RequiredString(System.String value) {
			string path = $"{ControllerPath}/required-string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateStringRequest(HttpMethod.Post, path, queryString, value)) {
				return this.GetRawResponse(request);
			}
		}

		public Task NullableString(System.String? value) {
			string path = $"{ControllerPath}/nullable-string";
			var queryString = new NameValueCollection();
			using (var request = this.CreateStringRequest(HttpMethod.Post, path, queryString, value)) {
				return this.GetRawResponse(request);
			}
		}

		public Task RequiredObjectArray(Test.Dto.MyDto[] array) {
			string path = $"{ControllerPath}/required-object-array";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.MyDto[]>(HttpMethod.Post, path, queryString, array)) {
				return this.GetRawResponse(request);
			}
		}

		public Task NullableObjectArray(Test.Dto.MyDto?[] array) {
			string path = $"{ControllerPath}/nullable-object-array";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.MyDto?[]>(HttpMethod.Post, path, queryString, array)) {
				return this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

