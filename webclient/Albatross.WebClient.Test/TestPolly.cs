using Albatross.Serialization;
using Albatross.Testing.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.WebClient.Test {
	public class MyRequest {
		public int Input { get; set; }
		public string Data { get; set; }
		public MyRequest(string data) {
			this.Data = data;
		}
	}

	public class MyResponse {
		public int Output { get; set; }
		public bool Success { get; set; }
	}

	public class PollyTestClient : ClientBase {
		public const System.String ControllerPath = "home";

		public PollyTestClient(ILogger logger, HttpClient client, IJsonSettings serializationOption) : base(logger, client, serializationOption) {
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

		public PollyTestClient2(ILogger logger, HttpClient client, IJsonSettings serializationOption) : base(logger, client, serializationOption) {
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

			var response = await myRetryPolicy.ExecuteAsync(async () => {
				using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
					return await this.client.SendAsync(request);
				}
			});
			return await ProcessResponseAsText(response);
		}

		public async Task<MyResponse?> PostData(MyRequest @myRequest) {
			string path = $"{ControllerPath}/polly-post-test";
			var queryString = new System.Collections.Specialized.NameValueCollection();
			using (var request = this.CreateJsonRequest<MyRequest>(HttpMethod.Post, path, queryString, @myRequest)) {
				return await this.GetJsonResponse<MyResponse>(request);
			}
		}

		public async Task<MyResponse?> PostDataWithRetry(MyRequest @myRequest) {
			var policy = this.GetDefaultRetryPolicy<MyResponse?>(args => false, nameof(PostData), true, 3, int.MaxValue);
			return await policy.ExecuteAsync(() => this.PostData(myRequest));
		}
	}

	public class TestPolly {
		public static readonly AsyncRetryPolicy<HttpResponseMessage> retryPolicy = Policy.Handle<HttpRequestException>()
			.OrResult<HttpResponseMessage>(response => (int)response.StatusCode >= 500 || response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
			.WaitAndRetryAsync(new[] {
					TimeSpan.FromSeconds(1),
					TimeSpan.FromSeconds(2),
					TimeSpan.FromSeconds(3)
			},
			(result, timespan) => { });

		IHost Create() {
			return new TestHostBuilder().RegisterServices((configuration, services) => {
				RegisterServices(configuration, services);
			}).Build();
		}
		void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			services.AddHttpClient<PollyTestClient>((provider, client) => {
				client.BaseAddress = new Uri("http://localhost:50000");
			}).AddPolicyHandler(retryPolicy);

			services.AddHttpClient<PollyTestClient2>((provider, client) => {
				client.BaseAddress = new Uri("http://localhost:50000");
			});
		}

		[Fact(Skip = "require web server")]
		public async Task RunNormalTest() {
			using var host = Create();
			var client = host.Services.GetRequiredService<PollyTestClient>();
			var result = await client.GetData(4);
			Assert.Equal("successful", result);
		}

		[Fact(Skip = "require web server")]
		public async Task RunFailTest() {
			using var host = Create();
			var client = host.Services.GetRequiredService<PollyTestClient>();
			await Assert.ThrowsAsync<ServiceException>(() => client.GetData(100));
		}

		[Fact(Skip = "require web server")]
		public async Task RunNormalTestWithManualPolicy() {
			using var host = Create();
			var client = host.Services.GetRequiredService<PollyTestClient2>();
			var result = await client.GetData(4);
			Assert.Equal("successful", result);
		}

		[Fact]
		public async Task TestPollyWrap() {
			var argumentExceptionRetryCount = 0;
			var invalidOpRetryCount = 0;

			var policy1 = Policy.Handle<ArgumentException>().WaitAndRetryAsync(3, retryCount => TimeSpan.FromMilliseconds(50), (result, timeSpan, retryCount, context) => {
				argumentExceptionRetryCount++;
			});
			var policy2 = Policy.Handle<InvalidOperationException>().WaitAndRetryAsync(5, retryCount => TimeSpan.FromMilliseconds(100), (result, timeSpan, retryCount, context) => {
				invalidOpRetryCount++;
			});
			var wrap = Policy.WrapAsync(policy1, policy2);
			await wrap.ExecuteAsync(() => Task.CompletedTask);
			Assert.Equal(0, argumentExceptionRetryCount);
			Assert.Equal(0, invalidOpRetryCount);
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			await Assert.ThrowsAsync<ArgumentException>(() => wrap.ExecuteAsync(() => throw new ArgumentException()));
			stopwatch.Stop();
			Assert.Equal(3, argumentExceptionRetryCount);
			Assert.Equal(0, invalidOpRetryCount);
			Assert.True(stopwatch.ElapsedMilliseconds >= 150 && stopwatch.ElapsedMilliseconds < 200);

			stopwatch.Restart();
			await Assert.ThrowsAsync<InvalidOperationException>(() => wrap.ExecuteAsync(() => throw new InvalidOperationException()));
			stopwatch.Stop();
			Assert.Equal(3, argumentExceptionRetryCount);
			Assert.Equal(5, invalidOpRetryCount);
			Assert.True(stopwatch.ElapsedMilliseconds >= 500);
		}
	}
}