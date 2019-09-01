using Albatross.Config.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config {
	public abstract class GetConfig<T> : IGetConfig<T> {
		protected IGetConfigValue getConfigValue;
		protected abstract string Key { get; }

		public GetConfig(IGetConfigValue getConfigValue) {
			this.getConfigValue = getConfigValue;
		}

		public virtual bool Required => true;

		public virtual T Get() {
			T t = this.getConfigValue.Get<T>(Key);
			if(ReferenceEquals(t, null) && Required) {
				throw new ConfigurationException(Key);
			} else {
				(t as IConfigSetting)?.Validate();
				return t;
			}
		}
	}
}
