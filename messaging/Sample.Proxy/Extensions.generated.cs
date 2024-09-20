using Microsoft.Extensions.DependencyInjection;

#nullable enable
namespace Sample.Proxy {
	public static partial class Extensions {
		public static IHttpClientBuilder AddClients(this IHttpClientBuilder builder) {
			return builder.AddTypedClient<CommandProxyService>().AddTypedClient<RunProxyService>().AddTypedClient<TestProxyService>();
		}
	}
}
#nullable disable

