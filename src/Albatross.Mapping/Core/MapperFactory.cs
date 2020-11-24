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

		public object Map(Type fromType, Type toType, object src) {
			Type serviceType = typeof(IMapper<,>).MakeGenericType(fromType, toType);
			object service = serviceProvider.GetRequiredService(serviceType);
			object dst = Activator.CreateInstance(toType);
			serviceType.GetMethod(nameof(IMapper<string, string>.Map)).Invoke(service, new object[] { src, dst });
			return dst;
		}
	}
}
