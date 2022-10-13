using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.WebClient.Test {
	public class PollyTestClient : ClientBase {
		public const System.String ControllerPath = "home";

		public PollyTestClient(ILogger logger, HttpClient client) : base(logger, client) {
		}

		public async Task<string> GetData(int count) {
			string path = $"{ControllerPath}/polly-test";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("count", count.ToString());

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				var result = await this.GetRawResponse(request);
				return result;
			}
		}
	}

	public class PollyTestClient2 : ClientBase {
		public const System.String ControllerPath = "home";

		public PollyTestClient2(ILogger logger, HttpClient client) : base(logger, client) {
		}

		public async Task<string> GetData(int count) {
			string path = $"{ControllerPath}/polly-test";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			queryString.Add("count", count.ToString());

			var myRetryPolicy = Policy.Handle<HttpRequestException>()
				.OrResult<HttpResponseMessage>(response => (int)response.StatusCode >= 500 || response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
				.WaitAndRetryAsync(new[] {
					TimeSpan.FromSeconds(1),
					TimeSpan.FromSeconds(2),
					TimeSpan.FromSeconds(3)
				},
				(result, timespan) => { });

			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				var result = await this.GetRawResponse(request, myRetryPolicy);
				return result;
			}
		}
	}

	public class PollyTestHost : TestHost {
		public static readonly AsyncRetryPolicy<HttpResponseMessage> retryPolicy = Policy.Handle<HttpRequestException>()
				.OrResult<HttpResponseMessage>(response => (int)response.StatusCode >= 500 || response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
				.WaitAndRetryAsync(new[] {
					TimeSpan.FromSeconds(1),
					TimeSpan.FromSeconds(2),
					TimeSpan.FromSeconds(3)
				},
				(result, timespan) => { });

		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddHttpClient<PollyTestClient>((provider, client) => {
				client.BaseAddress = new Uri("http://localhost:50000");
			}).AddPolicyHandler(retryPolicy);

			services.AddHttpClient<PollyTestClient2>((provider, client) => {
				client.BaseAddress = new Uri("http://localhost:50000");
			});
		}
	}

	public class TestPolly : IClassFixture<PollyTestHost> {
		private readonly PollyTestHost host;

		public TestPolly(PollyTestHost host) {
			this.host = host;
		}

		[Fact]
		public async Task RunNormalTest() {
			var client = host.Provider.GetRequiredService<PollyTestClient>();
			var result = await client.GetData(4);
			Assert.Equal("successful", result);
		}

		[Fact]
		public async Task RunFailTest() {
			var client = host.Provider.GetRequiredService<PollyTestClient>();
			await Assert.ThrowsAsync<ServiceException>(()=> client.GetData(100));
		}

		[Fact]
		public async Task RunNormalTestWithManualPolicy() {
			var client = host.Provider.GetRequiredService<PollyTestClient2>();
			var result = await client.GetData(4);
			Assert.Equal("successful", result);
		}
	}
}
