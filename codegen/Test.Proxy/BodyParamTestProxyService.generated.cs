using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class BodyParamTestProxyService : ClientBase {
		public BodyParamTestProxyService(ILogger<BodyParamTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/body-param-test";
		public Task FromBody1(Test.Dto.MyDto dto) {
			string path = $"{ControllerPath}/post";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.MyDto>(HttpMethod.Post, path, queryString, dto)) {
				return this.GetRawResponse(request);
			}
		}

		public Task FromBody2(Test.Dto.MyDto? dto) {
			string path = $"{ControllerPath}/post-with-nullable";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.MyDto>(HttpMethod.Post, path, queryString, dto)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<Test.Dto.MyClass> FromBody3(Test.Dto.MyDto dto) {
			string path = $"{ControllerPath}/post-with-return";
			var queryString = new NameValueCollection();
			using (var request = this.CreateJsonRequest<Test.Dto.MyDto>(HttpMethod.Post, path, queryString, dto)) {
				return this.GetRequiredJsonResponse<Test.Dto.MyClass>(request);
			}
		}
	}
}
#nullable disable

