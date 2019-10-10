using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Albatross.Config.UnitTest {
	public class GameData {
		public int Count { get; set; }
	}

	public class MySetting {
		public string Name { get; set; }
		public GameData Data { get; set; }
		public const string Key = "my";
	}

	public class GetMySetting : GetConfig<MySetting> {
		public GetMySetting(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => MySetting.Key;
	}
}
