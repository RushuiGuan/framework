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
			new SetupConfig(this.GetType().Assembly).RegisterServices(svc);
            svc.AddTransient<GetGoogleUrl>();
            svc.AddTransient<GetRequiredConfig>();
            provider = svc.BuildServiceProvider();
        }

        [Test]
		public void TestGetAssemblyLocation_NullInput() {
			var handle = new GetEntryAssemblyLocation(null);
			Assert.AreEqual(new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath, handle.CodeBase);
		}

		[Test]
		public void TestGetAssemblyLocation() {
            Assembly asm = this.GetType().Assembly;
			var handle = new GetEntryAssemblyLocation(asm);
			Assert.AreEqual(new Uri(asm.CodeBase).LocalPath, handle.CodeBase);
		}

        [Test]
        public void TestGetProgramSetting() {
			var setting = provider.GetRequiredService<ProgramSetting>();
            Assert.NotNull(setting);
            Assert.IsNotEmpty(setting.App);
            Assert.IsNotEmpty(setting.Group);
            Assert.IsNotEmpty(setting.ConfigDatabaseConnection);
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