using Albatross.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Hosting {
	public class AngularConfig : ConfigBase{
		public const string Key = "angular";
		public string[] Location { get; set; }
		public string[] Transformations { get; set; }

		public override void Init(IConfiguration configuration) {
		}
	}

	public class GetAngularConfig : GetConfig<AngularConfig> {
		public GetAngularConfig(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => AngularConfig.Key;
	}
}
