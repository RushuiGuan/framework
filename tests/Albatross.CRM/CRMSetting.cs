using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM {
	public class CRMSetting {
		public const string Key = "crm";
		public string DatabaseProvider { get; set; }
		public string ConnectionString { get; set; }
	}
}
