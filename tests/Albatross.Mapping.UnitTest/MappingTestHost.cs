using Albatross.Mapping.Core;
using Albatross.Mapping.ByAutoMapper;
using Albatross.Host.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Mapping.UnitTest {
	public class MappingTestHost : TestHost {
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddMapping(this.GetType().Assembly);
			services.AddAutoMapperMapping();
		}
	}
}
