using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System;
using Albatross.Mapping.Core;
using Albatross.Reflection;

namespace Albatross.Mapping.ByAutoMapper {
	public static class Extension {
		public static IServiceCollection AddAutoMapperMapping(this IServiceCollection services) {
			services.AddSingleton<ConfigAutoMapper>();
			services.AddSingleton(provider => provider.GetRequiredService<ConfigAutoMapper>().Create());
			services.AddSingleton(typeof(IMapper<,>), typeof(AutoMapperGeneric<,>));
			return services;
		}

		public static IServiceCollection AddProfiles(this IServiceCollection services, Assembly assembly) {
			foreach (Type type in assembly.GetConcreteClasses<AutoMapper.Profile>()) {
				services.AddTransient(typeof(AutoMapper.Profile), type);
			}
			return services;
		}
		public static IServiceCollection AddConfigMapping<T>(this IServiceCollection services) where T: class, IConfigMapping {
			services.AddSingleton<IConfigMapping, T>();
			return services;
		}
	}
}