using Microsoft.Extensions.DependencyInjection;

#nullable enable
namespace Test.WithInterface.Proxy {
	public static partial class Extensions {
		public static IHttpClientBuilder AddClients(this IHttpClientBuilder builder) {
			return builder.AddTypedClient<IArrayParamTestProxyService, ArrayParamTestProxyService>().AddTypedClient<ICancellationTokenTestProxyService, CancellationTokenTestProxyService>().AddTypedClient<IControllerRouteTestProxyService, ControllerRouteTestProxyService>().AddTypedClient<IFilteredMethodProxyService, FilteredMethodProxyService>().AddTypedClient<IFromBodyParamTestProxyService, FromBodyParamTestProxyService>().AddTypedClient<IFromQueryParamTestProxyService, FromQueryParamTestProxyService>().AddTypedClient<IFromRouteParamTestProxyService, FromRouteParamTestProxyService>().AddTypedClient<IHttpMethodTestProxyService, HttpMethodTestProxyService>().AddTypedClient<INullableParamTestProxyService, NullableParamTestProxyService>().AddTypedClient<INullableReturnTypeTestProxyService, NullableReturnTypeTestProxyService>().AddTypedClient<IRequiredParamTestProxyService, RequiredParamTestProxyService>().AddTypedClient<IRequiredReturnTypeTestProxyService, RequiredReturnTypeTestProxyService>();
		}
	}
}
#nullable disable

