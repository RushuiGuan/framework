using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Albatross.Framework.Utility {
	[Verb("null-test")]
	public class NullTestOption{
	}

	public class NullTest: MyUtilityBase<NullTestOption> {

		public NullTest(NullTestOption option):base(option) {
		}


		public Task<int>? RunUtility() {
			return null;
		}
	}
}
