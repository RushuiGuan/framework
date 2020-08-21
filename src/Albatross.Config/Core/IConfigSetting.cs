using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.Core {
	public interface IConfigSetting {
		void Validate();
		void Init(IConfiguration configuration);
	}
}
