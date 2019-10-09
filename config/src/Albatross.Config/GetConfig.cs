using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config {
	public abstract class GetConfig<T> : IGetConfig<T> {
		private readonly IConfiguration configuration;

		protected abstract string Key { get; }

		public GetConfig(IConfiguration configuration) {
			this.configuration = configuration;
		}

		public virtual bool Required => true;

		public virtual T Get() {
			var section = this.configuration.GetSection(Key);
			if (section == null && Required) {
				throw new ConfigurationException(Key);
			} else {
				T t = section.Get<T>();
				(t as IConfigSetting)?.Validate();
				return t;
			}
		}
	}
}
