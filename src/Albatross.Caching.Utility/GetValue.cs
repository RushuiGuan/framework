using Albatross.Hosting.Utility;
using CommandLine;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Albatross.Caching.Utility {
	[Verb("get")]
	public class GetValueOption : BaseOption {
		[Option('k', "key", Required = true)]
		public string Key { get; set; } = null!;
	}

	public class GetValue : MyUtilityBase<GetValueOption> {
		public GetValue(GetValueOption option) : base(option) {
		}
		public async Task<int> RunUtility(IRedisConnection svc) {
			var value = await svc.Database.StringGetAsync(new RedisKey(Options.Key));
			Options.WriteOutput(value.Cast<string>());
			return 0;
		}
	}
}
