using System.Linq;
using Albatross.Reflection;
using Albatross.Mapping.ByAutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System;

namespace Albatross.Mapping.Core {
    public static class Extension {

        public static IServiceCollection AddMapping(this IServiceCollection services, Assembly assembly = null) {
            services.AddSingleton<ConfigAutoMapper>();
            services.AddSingleton(provider => provider.GetRequiredService<ConfigAutoMapper>().Create());
            services.AddSingleton(typeof(IMapper<,>), typeof(AutoMapperGeneric<,>));
            services.AddSingleton<IMapperFactory, MapperFactory>();

            if (assembly != null) {
                foreach (Type type in assembly.GetConcreteClasses<AutoMapper.Profile>()) {
                    services.AddTransient(typeof(AutoMapper.Profile), type);
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
