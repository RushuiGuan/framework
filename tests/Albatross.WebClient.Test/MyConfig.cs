﻿using Albatross.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.WebClient.Test {
	public class MyConfig : ConfigBase {
		public MyConfig(IConfiguration configuration) : base(configuration) {
			this.TestUrl = configuration.GetEndPoint("test") ?? throw new ConfigurationException("endpoint:test");
		}
		public string TestUrl { get; private set; }
	}
}
