using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config {
	public abstract class GetConfig<Cfg> : IGetConfig<Cfg> {
		protected readonly IConfiguration configuration;

		protected abstract string Key { get; }

		public GetConfig(IConfiguration configuration) {
			this.configuration = configuration;
		}

		public virtual bool Required => true;

		public virtual Cfg Get() {
			Cfg t = default;
			var section = this.configuration.GetSection(Key);
			if (section != null) {
				t = section.Get<Cfg>();
				if(t is IConfigSetting) {
					((IConfigSetting)t).Init(configuration);
					((IConfigSetting)t).Validate();
				}
			}
			if (object.ReferenceEquals(t, null) && Required) {
				throw new ConfigurationException(Key);
			}
			return t;
		}


		protected string GetConnectionString(string name) {
			return configuration.GetConnectionString(name);
		}
	}
}
