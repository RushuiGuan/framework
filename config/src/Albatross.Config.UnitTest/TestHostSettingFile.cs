using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.UnitTest {
	[TestFixture]
	public class TestHostSettingFile {

		[Test]
		public void Run() {
			new SetupConfig(this.GetType().Assembly, useHostSetting: true);
		}
	}
}
