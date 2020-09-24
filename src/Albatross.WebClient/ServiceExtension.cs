using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Albatross.WebClient {
	public static class ServiceExtension {
		/// <summary>
		/// return named string specified in the endpoints section of the json configuration
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="name">endpoint name</param>
		/// <returns></returns>
		public static string GetEndPoint(this IConfiguration configuration, string name) => configuration.GetSection($"endpoints:{name}").Value;
	}
}
