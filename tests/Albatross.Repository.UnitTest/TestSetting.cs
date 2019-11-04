using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.UnitTest {
	public class TestSetting {
		public const string Key = "test";
		public string DatabaseProvider { get; set; }
		public string ConnectionString { get; set; }
	}
}
