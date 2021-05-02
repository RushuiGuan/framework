using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albatross.CommandQuery;

namespace Albatross.Test.CommandQuery {
	public class CommandTestHost :Albatross.Hosting.Test.TestHost{
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services
				.AddCommandBus()
				.AddCommand<MyCommand>()
				.AddCommandHandler<MyCommandHandler>();
		}
	}
}
