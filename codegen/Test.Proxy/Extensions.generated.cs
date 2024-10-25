using Microsoft.Extensions.DependencyInjection;

#nullable enable
namespace Test.Proxy {
	public static partial class Extensions {
		public static IHttpClientBuilder AddClients(this IHttpClientBuilder builder) {
			return builder.AddTypedClient<ArrayParamTestProxyService>().AddTypedClient<CancellationTokenTestProxyService>().AddTypedClient<ControllerRouteTestProxyService>().AddTypedClient<FilteredMethodProxyService>().AddTypedClient<FromBodyParamTestProxyService>().AddTypedClient<FromQueryParamTestProxyService>().AddTypedClient<FromRouteParamTestProxyService>().AddTypedClient<HttpMethodTestProxyService>().AddTypedClient<NullableParamTestProxyService>().AddTypedClient<NullableReturnTypeTestProxyService>().AddTypedClient<RequiredParamTestProxyService>().AddTypedClient<RequiredReturnTypeTestProxyService>();
		}
	}
}
#nullable disable

