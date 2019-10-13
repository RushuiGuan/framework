using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.IO;

namespace Albatross.Config.UnitTest {
	[Category(nameof(Albatross.Config))]
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

		[Test]
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
				Assert.AreEqual(1, value);
				SetValue(2);
				System.Threading.Thread.Sleep(1000);
				value = handle.Get();
				Assert.AreEqual(2, value);
			}
		}


		[Test]
		public void ConfigChangeCannotOverrideEnvironment() {
			int value = 99999;
			System.Environment.SetEnvironmentVariable("change-test", value.ToString());
			ServiceCollection svc = new ServiceCollection();
			new SetupConfig(this.GetType().GetAssemblyLocation()).RegisterServices(svc);
			svc.AddSingleton<GetChangeTest>();
			using (var provider = svc.BuildServiceProvider()) {
				var handle = provider.GetRequiredService<GetChangeTest>();
				Assert.AreEqual(value, handle.Get());
				SetValue(1);
				System.Threading.Thread.Sleep(1000);
				Assert.AreEqual(value, handle.Get());
				SetValue(2);
				System.Threading.Thread.Sleep(1000);
				Assert.AreEqual(value, handle.Get());
			}
		}
	}
}