using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.CommandQuery {
	public static class Extensions {
		public static IServiceCollection AddCommand<T>(this IServiceCollection services, Func<T, string> getQueueName) {
			return services;
		}

		public static IServiceCollection AddCommandHandler<T, >(this IServiceCollection services) {
			services.AddSingleton<ICommandHandler<T>, 
			return services;
		}
	}
}
