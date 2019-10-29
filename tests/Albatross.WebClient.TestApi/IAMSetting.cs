using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.WebClient.TestApi {
	public class IAMSetting {
		public const string Key = "iam";

		public string ConnectionString { get; set; }
		public string UserSalt { get; set; }
		public string DatabaseProvider { get; set; }
	}
}
