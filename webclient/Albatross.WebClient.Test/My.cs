using Albatross.Testing.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Albatross.RestClient.Test {
	public static class My {
		public static IHost Create() {
			return new TestHostBuilder().WithAppSettingsConfiguration("test")
				.RegisterServices((configuration, services) => {

				}).Build();
		}
	}
}