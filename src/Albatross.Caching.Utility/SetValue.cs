using CommandLine;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Caching.Utility {
	[Verb("set")]
	public class SetValueOption {
		[Option('k', "key", Required = true)]
		public string Key { get; set; } = null!;

		[Option('v', "value", Required = true)]
		public string Value { get; set; } = null!;
	}

	public class SetValue : MyUtilityBase<SetValueOption> {
		public SetValue(SetValueOption option) : base(option) {
		}
		public async Task<int> RunUtility(IRedisConnection svc) {
			var success = await svc.Database.StringSetAsync(new RedisKey(Options.Key), new RedisValue(Options.Value));
			return success? 0 : -1;
		}
	}
}
