using Albatross.Mapping.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Mapping.ByAutoMapper
{
    internal class MapperFactory : IMapperFactory
    {
        IServiceProvider serviceProvider;
        public MapperFactory(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }
        public IMapper<From, To> Get<From, To>()
        {
            return (IMapper<From, To>)this.serviceProvider.GetRequiredService(typeof(IMapper<From, To>));
        }
    }
}
