
using System;

namespace Albatross.Mapping.Core {
	public interface IMapper<From, To> {
		void Map(From src, To dst);
    }
}
