using Albatross.Caching;
using Albatross.Caching.BuiltIn;
using Microsoft.AspNetCore.Mvc;
using Sample.Caching.WebApi.CacheKeys;

namespace Sample.Caching.WebApi.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase {
		private readonly OneDayCache<string, Tier1Key> t1Cache;
		private readonly OneDayCache<string, Tier2Key> t2Cache;
		private readonly OneDayCache<string, Tier3Key> t3Cache;
		private readonly ICacheKeyManagement keyMgmt;

		public ValuesController(OneDayCache<string, Tier1Key> t1,
			OneDayCache<string, Tier2Key> t2,
			OneDayCache<string, Tier3Key> t3,
			ICacheKeyManagement keyMgmt) {
			this.t1Cache = t1;
			this.t2Cache = t2;
			this.t3Cache = t3;
			this.keyMgmt = keyMgmt;
		}

		[HttpGet("keys")]
		public string[] GetKeys() => keyMgmt.FindKeys("*");

		[HttpGet("t1")]
		public async Task<string> GetTier1([FromQuery] int keyValue) {
			var result = await t1Cache.ExecuteAsync(context => Task.FromResult(Random.Shared.Next().ToString()), new Tier1Key(keyValue));
			return result;
		}
		[HttpPost("remove-t1-item")]
		public void RemoveTier1Item([FromQuery] int keyValue) => keyMgmt.Remove(new Tier1Key(keyValue));
		[HttpPost("reset-t1-item")]
		public void ResetTier1Item([FromQuery] int keyValue) => keyMgmt.RemoveSelfAndChildren(new Tier1Key(keyValue));
		[HttpPost("reset-t1")]
		public void ResetTier1() => keyMgmt.Reset(new Tier1Key(0));


		[HttpGet("t2")]
		public async Task<string> GetTier2([FromQuery] int tier1Key, [FromQuery] int tier2Key) {
			var result = await t2Cache.ExecuteAsync(context => Task.FromResult(Random.Shared.Next().ToString()), new Tier2Key(tier1Key, tier2Key));
			return result;
		}
		[HttpPost("remove-t2-item")]
		public void RemoveTier2Item([FromQuery] int tier1Key, [FromQuery] int tier2Key) => keyMgmt.Remove(new Tier2Key(tier1Key, tier2Key));
		[HttpPost("reset-t2-item")]
		public void ResetTier2Item([FromQuery] int tier1Key, [FromQuery] int tier2Key) => keyMgmt.RemoveSelfAndChildren(new Tier2Key(tier1Key, tier2Key));
		[HttpPost("reset-t2")]
		public void ResetTier2([FromQuery]int tier1Key) => keyMgmt.Reset(new Tier2Key(tier1Key, 0));

		[HttpGet("t3")]
		public async Task<string> GetTier3([FromQuery] int tier1Key, [FromQuery] int tier2Key, [FromQuery] int tier3Key) {
			var result = await t3Cache.ExecuteAsync(context => Task.FromResult(Random.Shared.Next().ToString()), new Tier3Key(tier1Key, tier2Key, tier3Key));
			return result;
		}
		[HttpPost("remove-t3-item")]
		public void RemoveTier3Item([FromQuery] int tier1Key, [FromQuery] int tier2Key, [FromQuery] int tier3Key) => keyMgmt.Remove(new Tier3Key(tier1Key, tier2Key, tier3Key));
		[HttpPost("reset-t3-item")]
		public void ResetTier3Item([FromQuery] int tier1Key, [FromQuery] int tier2Key, [FromQuery] int tier3Key) => keyMgmt.RemoveSelfAndChildren(new Tier3Key(tier1Key, tier2Key, tier3Key));
		[HttpPost("reset-t3")]
		public void ResetTier3([FromQuery] int tier1Key, [FromQuery] int tier2Key) => keyMgmt.Reset(new Tier3Key(tier1Key, tier2Key, 0));
	}
}
