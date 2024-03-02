using Albatross.Caching;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Caching.WebApi.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase {
		private readonly Tier1CacheMgmt t1Cache;
		private readonly Tier2CacheMgmt t2Cache;
		private readonly Tier3CacheMgmt t3Cache;
		private readonly ICacheKeyManagement keyMgmt;

		public ValuesController(Tier1CacheMgmt t1, Tier2CacheMgmt t2, Tier3CacheMgmt t3, ICacheKeyManagement keyMgmt) {
			this.t1Cache = t1;
			this.t2Cache = t2;
			this.t3Cache = t3;
			this.keyMgmt = keyMgmt;
		}

		[HttpGet("keys")]
		public string[] GetKeys() => keyMgmt.FindKeys("*");

		[HttpGet("t1")]
		public async Task<string> GetTier1(int key) {
			var result = await t1Cache.ExecuteAsync(context => Task.FromResult(Random.Shared.Next().ToString()), new MultiTierKey(key));
			return result;
		}
		[HttpPost("remove-t1-item")]
		public void RemoveTier1Item(int key) => t1Cache.Remove(new MultiTierKey(key));
		[HttpPost("reset-t1-item")]
		public void ResetTier1Item(int key) => t1Cache.RemoveSelfAndChildren(new MultiTierKey(key));
		[HttpPost("reset-t1")]
		public void ResetTier1() => t1Cache.RemoveSelfAndChildren(new MultiTierKey());


		[HttpGet("t2")]
		public async Task<string> GetTier2(int t1Key, int key) {
			var result = await t2Cache.ExecuteAsync(context => Task.FromResult(Random.Shared.Next().ToString()), new MultiTierKey(t1Key, key));
			return result;
		}
		[HttpPost("remove-t2-item")]
		public void RemoveTier2Item(int t1Key, int key) => t2Cache.Remove(new MultiTierKey(t1Key, key));
		[HttpPost("reset-t2-item")]
		public void ResetTier2Item(int t1Key, int key) => t2Cache.RemoveSelfAndChildren(new MultiTierKey(t1Key, key));
		[HttpPost("reset-t2")]
		public void ResetTier2(int t1Key) => t2Cache.RemoveSelfAndChildren(new MultiTierKey(t1Key));

		[HttpGet("t3")]
		public async Task<string> GetTier3(int t1Key, int t2Key, int key) {
			var result = await t3Cache.ExecuteAsync(context => Task.FromResult(Random.Shared.Next().ToString()), new MultiTierKey(t1Key, t2Key, key));
			return result;
		}
		[HttpPost("remove-t3-item")]
		public void RemoveTier3Item(int t1Key, int t2Key, int key) => t3Cache.Remove(new MultiTierKey(t1Key, t2Key, key));
		[HttpPost("reset-t3-item")]
		public void ResetTier3Item(int t1Key, int t2Key, int key) => t3Cache.RemoveSelfAndChildren(new MultiTierKey(t1Key, t2Key, key));
		[HttpPost("reset-t3")]
		public void ResetTier3(int t1Key, int t2Key) => t3Cache.RemoveSelfAndChildren(new MultiTierKey(t1Key, t2Key));
	}
}
