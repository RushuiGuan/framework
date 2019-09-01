using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.Config {
	public class GetJsonConfigValueByDotnetCoreConfiguration : IGetConfigValue {
		IConfiguration cfg;
		public GetJsonConfigValueByDotnetCoreConfiguration(IConfiguration cfg) {
			this.cfg = cfg;
		}

		public T Get<T>(string key) {
			var section = cfg.GetSection(key);
			return section.Get<T>();
		}

		public string GetText(string key) {
			var section = cfg.GetSection(key);
			return section.Get<string>();
		}
	}
}
