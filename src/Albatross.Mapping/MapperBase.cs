using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Albatross.Mapping {
	public abstract class MapperBase<From, To> : Core.IMapper<From, To> {
		public abstract void Map(From src, To dst);

		public void Map(object src, object dst) {
			if(src is From && dst is To){
				Map((From)src, (To)dst);
			}else{
				throw new ArgumentException();
			}
		}
	}
}
