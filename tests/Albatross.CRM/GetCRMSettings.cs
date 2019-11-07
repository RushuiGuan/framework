using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Albatross.CRM {
	public class GetCRMSettings : GetConfig<CRMSetting> {
		public GetCRMSettings(IConfiguration configuration) : base(configuration) { }
		protected override string Key => CRMSetting.Key;
	}
}
