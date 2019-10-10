using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.IO;

namespace Albatross.Config.UnitTest {
    [Category(nameof(Albatross.Config))]
    public class TestConfigReload {
        ServiceCollection svc = new ServiceCollection();
        ServiceProvider provider;
        [OneTimeSetUp]
        public void Setup() {
			new SetupConfig(this.GetType().Assembly).RegisterServices(svc);
            svc.AddSingleton<GetChangeTest>();
            provider = svc.BuildServiceProvider();
        }

        private void SetValue(int value) {
            var location = provider.GetRequiredService<IGetEntryAssemblyLocation>().Directory;
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
        public void Run() {
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
}