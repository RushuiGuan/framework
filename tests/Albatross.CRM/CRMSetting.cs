using Albatross.Repository.Core;

namespace Albatross.CRM {
	public class CRMSetting  {
		public const string Key = "crm";
		public string ConnectionString { get; set; }
		public string DatabaseProvider { get; set; }
	}
}
