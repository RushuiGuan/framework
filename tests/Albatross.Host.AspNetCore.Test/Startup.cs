using System.Diagnostics;
using Albatross.Config.Core;
using Albatross.Host.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Host.AspNetCore.Test {
	/// <summary>
	/// </summary>
	public class Startup : Albatross.Host.AspNetCore.Startup {
		public Startup(IConfiguration configuration) : base(configuration) {
		}
	}
}