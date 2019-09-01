using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Test.AspNetCore
{
    public class Startup : Albatross.Host.AspNetCore.Startup {
		public Startup(IConfiguration configuration) : base(configuration) {
		}
	}
}