using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Mapping.Core {
	internal class MapperFactory : IMapperFactory {
		IServiceProvider serviceProvider;
		public MapperFactory(IServiceProvider serviceProvider) {
			this.serviceProvider = serviceProvider;
		}
		public IMapper<From, To> Get<From, To>() {
			return (IMapper<From, To>)this.serviceProvider.GetRequiredService(typeof(IMapper<From, To>));
		}
	}
}
