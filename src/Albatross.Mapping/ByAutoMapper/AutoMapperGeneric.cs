using Albatross.Mapping.Core;
using AutoMapper;

namespace Albatross.Mapping.ByAutoMapper {
	public class AutoMapperGeneric<From, To> : IMapper<From, To> {
		IMapper mapper;

		public AutoMapperGeneric(IMapper mapper) {
			this.mapper = mapper;
		}

		public void Map(From src, To dst) {
			mapper.Map<From, To>(src, dst);
		}
	}
}
