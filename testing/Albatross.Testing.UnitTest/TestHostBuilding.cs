using Albatross.Testing.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Sample.Hosting.Test {
	public class TestHostBuilding {
		[Fact]
		public void TestILogger() {
			using var host = new TestHostBuilder().WithLogging().Build();
			host.Services.GetService<ILogger<TestHostBuilding>>().Should().NotBeNull();
			host.Services.GetService<ILogger>().Should().NotBeNull();
		}

		[Fact]
		public void TestConfiguration() {
			using var host = new TestHostBuilder().WithAppSettingsConfiguration("test").Build();
			host.Services.GetService<IConfiguration>().Should().NotBeNull();
		}
	}
}