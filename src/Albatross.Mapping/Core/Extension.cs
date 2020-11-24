using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System;
using System.Data;

namespace Albatross.Mapping.Core {
	public static class Extension {

        public static IServiceCollection AddMapping(this IServiceCollection services, Assembly assembly = null) {
            services.AddSingleton<IMapperFactory, MapperFactory>();
            
			if (assembly != null) {
				Type genericDefinition = typeof(IMapper<,>);
                foreach (Type type in assembly.GetConcreteClasses()) {
					if (type.TryGetClosedGenericType(genericDefinition, out Type genericType)) {
						services.AddSingleton(genericType, type);
					}
                }
            }
            return services;
        }

        public static To Map<From, To>(this IMapper<From, To> mapper, From from) 
			where To:new(){
			To to = new To();
			mapper.Map(from, to);
			return to;
		}

		public static void Map<From, To>(this IMapperFactory factory, From from, To to){
			factory.Get<From, To>().Map(from, to);
		}

		public static To Map<From, To>(this IMapperFactory factory, From from)
			where To : new() {
			var mapper = factory.Get<From, To>();
			return mapper.Map(from);
		}
	}
}
