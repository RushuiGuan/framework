using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Albatross.Config {
	public abstract class ConfigBase : IConfigSetting {
		public abstract void Init(IConfiguration configuration);

		public virtual void Validate() {
			Validator.ValidateObject(this, new ValidationContext(this));
		}
	}
}
