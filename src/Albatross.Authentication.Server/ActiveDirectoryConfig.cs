using Albatross.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Authentication.Server {
	public class ActiveDirectoryConfig : ConfigBase{
		public const string Key = "activeDirectory";
		[Required]
		public string DomainName { get; set; }
		
		public string LdapString { get; set; }

		public override void Init(IConfiguration configuration) {
		}
	}
	public class GetActiveDirectoryConfig : GetConfig<ActiveDirectoryConfig> {
		public GetActiveDirectoryConfig(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => ActiveDirectoryConfig.Key;
	}
}
