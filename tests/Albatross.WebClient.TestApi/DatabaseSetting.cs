using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.WebClient.TestApi {
	public class DatabaseSetting {
		public const string Key = "database";

		public string ConnectionString { get; set; }
		public string DatabaseProvider { get; set; }
	}
}
