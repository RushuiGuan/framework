using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config {
	public abstract class GetConfig<T> : IGetConfig<T> {
		protected readonly IConfiguration configuration;

		protected abstract string Key { get; }

		public GetConfig(IConfiguration configuration) {
			this.configuration = configuration;
		}

		public virtual bool Required => true;

		public virtual T Get() {
			T t = default(T);
			var section = this.configuration.GetSection(Key);
			if (section != null) {
				t = section.Get<T>();
				(t as IConfigSetting)?.Validate();
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
