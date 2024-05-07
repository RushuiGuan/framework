using BoDi;
using Microsoft.Extensions.DependencyInjection;
using System;
using TechTalk.SpecFlow.Infrastructure;

namespace Albatross.SpecFlowPlugin {
	public class AlbatrossTestObjectResolver : ITestObjectResolver {
		public object ResolveBindingInstance(Type bindingType, IObjectContainer container) {
			if (container.IsRegistered<IServiceProvider>()) {
				var serviceProvider = container.Resolve<IServiceProvider>();
				return serviceProvider.GetRequiredService(bindingType);
			}

			if (container.IsRegistered<IServiceScope>()) {
				var scoped = container.Resolve<IServiceScope>();
				return scoped.ServiceProvider.GetRequiredService(bindingType);
			}

			return container.Resolve(bindingType);
		}
	}
}
