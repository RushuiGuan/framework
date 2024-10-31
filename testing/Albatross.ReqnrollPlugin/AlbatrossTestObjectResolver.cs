using Reqnroll.BoDi;
using Reqnroll.Infrastructure;
using System;

namespace Albatross.ReqnrollPlugin {
	public class AlbatrossTestObjectResolver : ITestObjectResolver {
		public object ResolveBindingInstance(Type bindingType, IObjectContainer container) {
			var serviceProvider = container.Resolve<IServiceProvider>();
			return serviceProvider.GetService(bindingType) ?? container.Resolve(bindingType);
		}
	}
}