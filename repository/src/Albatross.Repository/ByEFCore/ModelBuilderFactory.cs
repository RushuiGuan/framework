using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Albatross.Repository.ByEFCore {
	public interface IModelBuilderFactory {
		IEnumerable<IBuildEntityModel> Get(Assembly assembly);
	}

	public class ModelBuilderFactory : IModelBuilderFactory {
        IServiceProvider provider;

        public ModelBuilderFactory(IServiceProvider provider) {
            this.provider = provider;
        }

        public IEnumerable<IBuildEntityModel> Get(Assembly assembly){
            List<IBuildEntityModel> list = new List<IBuildEntityModel>();
			foreach (Type type in assembly.GetConcreteClasses<IBuildEntityModel>()) {
				list.Add((IBuildEntityModel)provider.GetRequiredService(type));
			}
            return list;
        }
    }
}
