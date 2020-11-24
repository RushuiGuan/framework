using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Mapping.Core
{
    public interface IMapperFactory
    {
        IMapper<From, To> Get<From, To>();
		object Map(Type from, Type to, object src);
    }
}
