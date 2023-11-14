using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Albatross.Caching.TestApi.Controllers {
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

		[HttpGet("tier1")]
		public async Task<string> GetTier1(string key) {
			var result = await t1Cache.ExecuteAsync(context => Task.FromResult(key.GetHashCode().ToString()), My.Context(key));
			return result;
		}
		
		[HttpPost("remove-tier1-item")]
		public void RemoveTier1Item(string key) => t1Cache.Remove(key);
		[HttpPost("reset-tier1-item")]
		public void ResetTier1Item(string key) => t1Cache.RemoveAll(key);
		[HttpPost("reset-tier1")]
		public void ResetTier1() => t1Cache.RemoveAll();


		[HttpGet("tier2")]
		public async Task<byte[]> GetTier2(string t1Key, string key) {
			var result = await t2Cache.ExecuteAsync(context => Task.FromResult(Encoding.UTF8.GetBytes(key)), My.Context(t1Key, key));
			return result;
		}
		[HttpPost("remove-t2-item")]
		public void RemoveTier2(string t1Key, string key) => t2Cache.Remove(t1Key, key);
		[HttpPost("reset-t2-item")]
		public void ResetTier2(string t1Key, string key) => t2Cache.RemoveAll(t1Key, key);
		[HttpPost("reset-yours")]
		public void ResetYours() => t2Cache.RemoveAll();

		[HttpGet("tier3")]
		public async Task<HisHerData> GetHis(string t1Key, string t2Key, string key) {
			var result = await t3Cache.ExecuteAsync(context => Task.FromResult(new HisHerData(key, Random.Shared.Next(0, 100))),
				My.Context(t1Key, t2Key, key));
			return result;
		}
		[HttpPost("remove-t3-item")]
		public void RemoveTier3Item(string t1Key, string t2Key, string key) => t3Cache.Remove(t1Key, t2Key, key);
		[HttpPost("reset-t3-item")]
		public void ResetTier3Item(string t1Key, string t2Key, string key) => t3Cache.RemoveAll(t1Key, t2Key, key);
		[HttpPost("reset-t3")]
		public void ResetTier3(string t1Key, string t2Key, string key) => t3Cache.RemoveAll(t1Key, t2Key);
	}
}
