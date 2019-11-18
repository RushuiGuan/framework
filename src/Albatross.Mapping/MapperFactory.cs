using Albatross.Mapping.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Albatross.Reflection;

namespace Albatross.Mapping {
	internal class MapperFactory : IMapperFactory {
		IServiceProvider serviceProvider;
		public MapperFactory(IServiceProvider serviceProvider) {
			this.serviceProvider = serviceProvider;
		}
		public IMapper<From, To> Get<From, To>() {
			return (IMapper<From, To>)this.serviceProvider.GetRequiredService(typeof(IMapper<From, To>));
		}

		public IMapper Get(Type fromType, Type toType) {
			Type type = typeof(IMapper<,>).MakeGenericType(fromType, toType);
			return (IMapper)this.serviceProvider.GetRequiredService(type);
		}

		public bool TryGet(Type fromType, Type toType, out IMapper mapper) {
			Type type = typeof(IMapper<,>).MakeGenericType(fromType, toType);
			mapper = (IMapper)this.serviceProvider.GetService(type);
			return mapper != null;
		}
	}
}
