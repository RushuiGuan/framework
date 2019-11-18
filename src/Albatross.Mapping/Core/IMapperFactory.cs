using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Mapping.Core {
	public interface IMapperFactory {
		IMapper<From, To> Get<From, To>();
		IMapper Get(Type fromType, Type toType);
		bool TryGet(Type fromType, Type toType, out IMapper mapper);
	}
}
