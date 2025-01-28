using Microsoft.Extensions.DependencyInjection;

#nullable enable
namespace Test.Proxy {
	public static partial class Extensions {
		public static IHttpClientBuilder AddClients(this IHttpClientBuilder builder) {
			return builder.AddTypedClient<ArrayParamTestProxyService>().AddTypedClient<CancellationTokenTestProxyService>().AddTypedClient<ControllerRouteTestProxyService>().AddTypedClient<CustomJsonSettingsProxyService>().AddTypedClient<DuplicateNameTestProxyService>().AddTypedClient<FilteredMethodProxyService>().AddTypedClient<FromBodyParamTestProxyService>().AddTypedClient<FromQueryParamTestProxyService>().AddTypedClient<FromRouteParamTestProxyService>().AddTypedClient<HttpMethodTestProxyService>().AddTypedClient<InterfaceAndAbstractClassTestProxyService>().AddTypedClient<NullableParamTestProxyService>().AddTypedClient<NullableReturnTypeTestProxyService>().AddTypedClient<OmittedConstructorProxyService>().AddTypedClient<RequiredParamTestProxyService>().AddTypedClient<RequiredReturnTypeTestProxyService>();
		}
	}
}
#nullable disable

