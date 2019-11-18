
using System;

namespace Albatross.Mapping.Core {
	public interface IMapper<From, To>: IMapper {
		void Map(From src, To dst);

    }

	public interface IMapper{
		void Map(object src, object dst);
	}
}

