using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Config {
	public abstract class ConfigBase  {
		public virtual string Key => string.Empty;
		public ConfigBase(IConfiguration configuration) {
			if (!string.IsNullOrEmpty(Key)) {
				var section = configuration.GetSection(Key);
				section.Bind(this);
			}
		}

		public virtual void Validate() {
			Validator.ValidateObject(this, new ValidationContext(this));
		}
	}
}
