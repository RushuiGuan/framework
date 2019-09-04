using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Albatross.Repository.ByEFCore {
    public static class ServiceExtension {
        public static IServiceCollection AddModelBuilderFactory(this IServiceCollection services) {
            services.AddSingleton<IModelBuilderFactory, ModelBuilderFactory>();
            return services;
        }

        public static IServiceCollection AddEntityMaps(this IServiceCollection services, Assembly assembly) {
			foreach(Type type in assembly.GetConcreteClasses<IBuildEntityModel>()){
				services.AddSingleton(type);
				services.AddSingleton<IBuildEntityModel>(provider => (IBuildEntityModel)provider.GetRequiredService(type));
			}
            return services;
        }

        public static IServiceCollection AddCustomEFCore(this IServiceCollection services, Assembly assembly) {
            services.AddModelBuilderFactory().AddEntityMaps(assembly);
            return services;
        }
    }
}
