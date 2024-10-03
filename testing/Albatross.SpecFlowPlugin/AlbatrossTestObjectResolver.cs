using BoDi;
using Microsoft.Extensions.DependencyInjection;
using System;
using TechTalk.SpecFlow.Infrastructure;

namespace Albatross.SpecFlowPlugin {
	public class AlbatrossTestObjectResolver : ITestObjectResolver {
		public object ResolveBindingInstance(Type bindingType, IObjectContainer container) {
			var serviceProvider = container.Resolve<IServiceProvider>();
			return serviceProvider.GetService(bindingType) ??container.Resolve(bindingType);
		}
	}
}
