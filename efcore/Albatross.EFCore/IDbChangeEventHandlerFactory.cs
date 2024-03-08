using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Albatross.EFCore {
	public interface IDbChangeEventHandlerFactory {
		IEnumerable<IDbChangeEventHandler> Create();
	}
	public class DbChangeEventHandlerFactory : IDbChangeEventHandlerFactory {
		private readonly IServiceProvider serviceProvider;

		public DbChangeEventHandlerFactory(IServiceProvider serviceProvider) {
			this.serviceProvider = serviceProvider;
		}
		public IEnumerable<IDbChangeEventHandler> Create() {
			return this.serviceProvider.GetRequiredService<IEnumerable<IDbChangeEventHandler>>();
		}
	}	
}
