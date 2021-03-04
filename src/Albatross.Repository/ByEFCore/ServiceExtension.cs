using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Albatross.Repository.ByEFCore {
	public static class ServiceExtension {

		public static IServiceCollection AddEntityMaps(this IServiceCollection services, Assembly assembly) {
			foreach (Type type in assembly.GetConcreteClasses<IBuildEntityModel>()) {
				services.AddSingleton(type);
				services.AddSingleton<IBuildEntityModel>(provider => (IBuildEntityModel)provider.GetRequiredService(type));
			}
			return services;
		}
		public static IEnumerable<IBuildEntityModel> GetEntityModels(this Assembly assembly) {
			List<IBuildEntityModel> list = new List<IBuildEntityModel>();
			foreach (Type type in assembly.GetConcreteClasses<IBuildEntityModel>()) {
				list.Add((IBuildEntityModel)Activator.CreateInstance(type));
			}
			return list;
		}
	}
}
