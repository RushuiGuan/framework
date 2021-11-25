using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Albatross.Hosting {
	public class AngularConfig : ConfigBase{
		public const string Key = "angular";

		public string[] ConfigFile { get; set; } = new string[0];
		public string[] BaseHrefFile { get; set; } = new string[0];
		/// <summary>
		/// Use by the hosting aspnetcore web app.  Default is String.Empty.  This property should reflect the relative path for the angular app to the aspnetcore web app.  It should start with a / but never end with one.
		/// For example, if the angular path is: http://localhost/demo/ui and the aspnetcore web app path is: http://localhost/demo, the request path should be /ui
		/// </summary>
		public string RequestPath { get; set; } = string.Empty;

		/// <summary>
		/// Use by angular.  This property should reflect the relative path for the angular app to the host.  It should always start with a / and end with a /.
		/// For example, if the angular path is: http://localhost/demo/ui  The BaseHref should be /demo/ui/
		/// </summary>
		public string BaseHref { get; set; } = "/";

		public override void Init(IConfiguration configuration) {
		}
	}

	public class GetAngularConfig : GetConfig<AngularConfig> {
		public GetAngularConfig(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => AngularConfig.Key;
	}
}
