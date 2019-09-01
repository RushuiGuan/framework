using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Host.AspNetCore {
	public interface IGetServerJsonSerializer {
		JsonSerializer Get();
	}
}