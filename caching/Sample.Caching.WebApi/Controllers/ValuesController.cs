using Albatross.Caching;
using Albatross.Caching.BuiltIn;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Caching.WebApi.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase {
		private readonly OneDayCache<string, Tier1Key> t1Cache;
		private readonly OneDayCache<string, Tier2Key> t2Cache;
		private readonly OneDayCache<string, Tier3Key> t3Cache;
		private readonly ICacheKeyManagement keyMgmt;

		public ValuesController(OneDayCache<string, Tier1Key> t1, OneDayCache<string, Tier2Key> t2, OneDayCache<string, Tier3Key> t3, ICacheKeyManagement keyMgmt) {
			this.t1Cache = t1;
			this.t2Cache = t2;
			this.t3Cache = t3;
			this.keyMgmt = keyMgmt;
		}

		[HttpGet("keys")]
		public string[] GetKeys() => keyMgmt.FindKeys("*");

		[HttpGet("t1")]
		public async Task<string> GetTier1(int key) {
			var result = await t1Cache.ExecuteAsync(context => Task.FromResult(Random.Shared.Next().ToString()), new Tier1Key(key));
			return result;
		}
		[HttpPost("remove-t1-item")]
		public void RemoveTier1Item(int key) => keyMgmt.Remove(new Tier1Key(key));
		[HttpPost("reset-t1-item")]
		public void ResetTier1Item(int key) => keyMgmt.RemoveSelfAndChildren(new Tier1Key(key));
		[HttpPost("reset-t1")]
		public void ResetTier1() => keyMgmt.Reset(new Tier1Key(0));


		[HttpGet("t2")]
		public async Task<string> GetTier2(int t1Key, int key) {
			var result = await t2Cache.ExecuteAsync(context => Task.FromResult(Random.Shared.Next().ToString()), new Tier2Key(t1Key, key));
			return result;
		}
		[HttpPost("remove-t2-item")]
		public void RemoveTier2Item(int t1Key, int key) => keyMgmt.Remove(new Tier2Key(t1Key, key));
		[HttpPost("reset-t2-item")]
		public void ResetTier2Item(int t1Key, int key) => keyMgmt.RemoveSelfAndChildren(new Tier2Key(t1Key, key));
		[HttpPost("reset-t2")]
		public void ResetTier2(int t1Key) => keyMgmt.Reset(new Tier2Key(t1Key, 0));

		[HttpGet("t3")]
		public async Task<string> GetTier3(int t1Key, int t2Key, int key) {
			var result = await t3Cache.ExecuteAsync(context => Task.FromResult(Random.Shared.Next().ToString()), new Tier3Key(t1Key, t2Key, key));
			return result;
		}
		[HttpPost("remove-t3-item")]
		public void RemoveTier3Item(int t1Key, int t2Key, int key) => keyMgmt.Remove(new Tier3Key(t1Key, t2Key, key));
		[HttpPost("reset-t3-item")]
		public void ResetTier3Item(int t1Key, int t2Key, int key) => keyMgmt.RemoveSelfAndChildren(new Tier3Key(t1Key, t2Key, key));
		[HttpPost("reset-t3")]
		public void ResetTier3(int t1Key, int t2Key) => keyMgmt.Reset(new Tier3Key(t1Key, t2Key, 0));
	}
}
