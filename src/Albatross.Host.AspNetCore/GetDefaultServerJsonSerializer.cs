using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Host.AspNetCore {
	public class GetDefaultServerJsonSerializer : IGetServerJsonSerializer {
		public JsonSerializer Get() {
			return new JsonSerializer {
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
			};
		}
	}
}