using Albatross.Config;
using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Albatross.Host.NUnit {
	public abstract class TestBase<T> where T : class, IDisposable {
        private ServiceCollection services = new ServiceCollection();
        private ServiceProvider provider;

        public virtual void RegisterPackages(IServiceCollection services) {
        }

		[OneTimeSetUp]
        public void InitializeTestFixture() {
			var cfg = new SetupConfig(this.GetType().Assembly).Configuration;
			services.AddSingleton<IConfiguration>(cfg);
            services.AddCustomConfig(this.GetType().Assembly, true);
            services.AddTransient<IServiceScope>(args => provider.CreateScope());
            services.AddTransient<T>();
            RegisterPackages(services);

            this.provider = services.BuildServiceProvider();


            using (var t = NewUnitOfWork()) {
                OneTimeSetUp(t);
            }
        }

        public virtual void OneTimeSetUp(T unitOfWork) { }
        public virtual void OneTimeTearDown(T unitOfWork) { }

        [OneTimeTearDown]
        public void ShutdownTestFixture() {
            using (var scope = provider.CreateScope()) {
                OneTimeTearDown(provider.GetRequiredService<T>());
            }
        }

        public virtual T NewUnitOfWork() {
            return provider.GetRequiredService<T>();
        }
        protected IServiceProvider Provider => provider;
    }
}