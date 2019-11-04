using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Reflection;

namespace Albatross.Config.UnitTest {
    [Category(nameof(Albatross.Config))]
    public class Tests {
        ServiceCollection svc = new ServiceCollection();
        ServiceProvider provider;
        [OneTimeSetUp]
        public void Setup() {
			new SetupConfig(this.GetType().GetAssemblyLocation()).RegisterServices(svc);
            svc.AddTransient<GetGoogleUrl>();
            svc.AddTransient<GetRequiredConfig>();
            provider = svc.BuildServiceProvider();
        }

        [Test]
        public void TestGetProgramSetting() {
			var setting = provider.GetRequiredService<ProgramSetting>();
            Assert.NotNull(setting);
            Assert.IsNotEmpty(setting.App);
            Assert.IsNotEmpty(setting.Group);
            Assert.IsNotEmpty(setting.Environment);
        }

        [Test]
        public void TestGetGoogleUrl() {
            var value = provider.GetRequiredService<GetGoogleUrl>().Get();
            Assert.IsNotEmpty(value);
        }

        [Test]
        public void TestGetRequiredConfig() {
            var handle = provider.GetRequiredService<GetRequiredConfig>();
            Assert.Catch<ConfigurationException>(() => handle.Get());
        }
    }
}