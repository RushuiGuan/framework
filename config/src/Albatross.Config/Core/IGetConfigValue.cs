using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.Core {
	public interface IGetConfigValue {
		T Get<T>(string key);
		string GetText(string key);
	}
}
