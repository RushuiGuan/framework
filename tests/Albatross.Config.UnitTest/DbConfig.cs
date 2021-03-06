﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.UnitTest {
	public class DbConfig : Albatross.Config.Core.IConfigSetting {
		public string DbConnection { get; set; }

		public void Init(IConfiguration configuration) {
			this.DbConnection = configuration.GetConnectionString("test");
		}

		public void Validate() {
		}
	}
	public class GetDbConfig : GetConfig<DbConfig> {
		public GetDbConfig(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => "db-config";
	}
}
