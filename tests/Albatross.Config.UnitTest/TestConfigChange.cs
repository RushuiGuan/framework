using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.IO;
using Xunit;

namespace Albatross.Config.UnitTest {
	public class TestConfigChange {
		private void SetValue(int value) {
			var location = this.GetType().GetAssemblyLocation();
			string path = Path.Join(location, "appsettings.json");
			string content;
			using (StreamReader reader = new StreamReader(path)) {
				content = reader.ReadToEnd();
			}
			JObject obj = JObject.Parse(content);
			obj["change-test"] = value;

			using (StreamWriter writer = new StreamWriter(path)) {
				writer.Write(obj.ToString());
			}
		}

		[Fact]
		public void NormalAppSettingChange() {
			System.Environment.SetEnvironmentVariable("change-test", null);
			ServiceCollection svc = new ServiceCollection();
			new SetupConfig(this.GetType().GetAssemblyLocation()).RegisterServices(svc);
			svc.AddSingleton<GetChangeTest>();
			using (var provider = svc.BuildServiceProvider()) {
				var handle = provider.GetRequiredService<GetChangeTest>();
				SetValue(1);
				System.Threading.Thread.Sleep(1000);
				int value = handle.Get();
				Assert.Equal(1, value);
				SetValue(2);
				System.Threading.Thread.Sleep(1000);
				value = handle.Get();
				Assert.Equal(2, value);
			}
		}


		[Fact]
		public void ConfigChangeCannotOverrideEnvironment() {
			int value = 99999;
			System.Environment.SetEnvironmentVariable("change-test", value.ToString());
			ServiceCollection svc = new ServiceCollection();
			new SetupConfig(this.GetType().GetAssemblyLocation()).RegisterServices(svc);
			svc.AddSingleton<GetChangeTest>();
			using (var provider = svc.BuildServiceProvider()) {
				var handle = provider.GetRequiredService<GetChangeTest>();
				Assert.Equal(value, handle.Get());
				SetValue(1);
				System.Threading.Thread.Sleep(1000);
				Assert.Equal(value, handle.Get());
				SetValue(2);
				System.Threading.Thread.Sleep(1000);
				Assert.Equal(value, handle.Get());
			}
		}
	}
}