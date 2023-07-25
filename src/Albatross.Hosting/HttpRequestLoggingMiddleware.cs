using Albatross.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Albatross.Hosting {
	public record class UsageData {
		public UsageData(HttpContext context) {
			User = GetCurrentUserFromHttpContext.GetFromContext(context);
			Url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}{context.Request.Path}{context.Request.QueryString}";
			Method = context.Request.Method;
			RemoteIpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "N.A.";
		}
		public string? User { get; set; }
		public string Url { get; set; }
		public string RemoteIpAddress { get; set; }
		public string Method { get; set; }
	}
	public class HttpRequestLoggingMiddleware {
		private readonly RequestDelegate next;

		public HttpRequestLoggingMiddleware(RequestDelegate next) {
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext context, ILogger<HttpRequestLoggingMiddleware> logger) {
			logger.LogInformation("{@data}", new UsageData(context));
			await next(context);
		}
	}
}
