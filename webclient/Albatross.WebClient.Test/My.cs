using Albatross.Testing.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Albatross.WebClient.Test {
	public static class My {
		public static IHost Create() {
			return new TestHostBuilder().WithAppSettingsConfiguration("test")
				.RegisterServices((configuration, services) => {
				services.AddTestClientService();
			}).Build();

		}
	}
}