using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config {
	public abstract class GetConfig<T> : IGetConfig<T> where T : class, new() {
		protected readonly IConfiguration configuration;

		protected abstract string Key { get; }

		public GetConfig(IConfiguration configuration) {
			this.configuration = configuration;
		}

		public virtual T Get() {
			T t = default;
			//section will never be null
			var section = this.configuration.GetSection(Key);
			t = section.Get<T>();
			// if the section exists, but the value is {}, the method will still return null
			if (t == null) { t = new T(); }
			if (t is IConfigSetting) {
				((IConfigSetting)t).Init(configuration);
				((IConfigSetting)t).Validate();
			}
			return t;
		}

		protected string GetConnectionString(string name) {
			return configuration.GetConnectionString(name);
		}
	}
}