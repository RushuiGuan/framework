using Albatross.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Albatross.Caching {
	public class CachingConfig {
		public const string Key = "caching";
		public string[] SiblingEndPoints { get; set; } = new string[0];
	}

	public class GetCachingConfig : GetConfig<CachingConfig> {
		public GetCachingConfig(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => CachingConfig.Key;
	}
}
