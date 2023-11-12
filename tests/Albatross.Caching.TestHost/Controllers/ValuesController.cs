using Polly;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Albatross.Caching.TestHost.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController: ControllerBase {
		private readonly ICacheManagementFactory factory;
		private readonly ICacheKeyManagement keyMgmt;

		public ValuesController(ICacheManagementFactory factory, ICacheKeyManagement keyMgmt) {
			this.factory = factory;
			this.keyMgmt = keyMgmt;
		}

		[HttpGet("mine")]
		public async Task<string> GetMine(string key) {
			var cacheMgmt = factory.Get<string>(nameof(MyCacheMgmt));
			var result = await cacheMgmt.ExecuteAsync(context => Task.FromResult(key.GetHashCode().ToString()), new Context(key));
			return result;
		}
		[HttpPost("remove-mine")]
		public void RemoveMine(string key) {
			var cacheMgmt = factory.Get<string>(nameof(MyCacheMgmt));
			cacheMgmt.Remove(key);
		}
		[HttpPost("reset-mine")]
		public void ResetMine() {
			var cacheMgmt = factory.Get<string>(nameof(MyCacheMgmt));
			cacheMgmt.Reset();
		}

		[HttpGet("yours")]
		public async Task<byte[]> GetYours(string key) {
			var cacheMgmt = factory.Get<byte[]>(nameof(YourCacheMgmt));
			var result = await cacheMgmt.ExecuteAsync(context => Task.FromResult(Encoding.UTF8.GetBytes(key)), new Context(key));
			return result;
		}
		[HttpPost("remove-yours")]
		public void RemoveYours(string key) {
			var cacheMgmt = factory.Get<byte[]>(nameof(YourCacheMgmt));
			cacheMgmt.Remove(key);
		}
		[HttpPost("reset-yours")]
		public void ResetYours() {
			var cacheMgmt = factory.Get<byte[]>(nameof(YourCacheMgmt));
			cacheMgmt.Reset();
		}

		[HttpGet("his")]
		public async Task<HisHerData> GetHis(string key) {
			var cacheMgmt = factory.Get<HisHerData>(nameof(HisCacheMgmt));
			var result = await cacheMgmt.ExecuteAsync(context => Task.FromResult(new HisHerData(key, Random.Shared.Next(0, 100))), new Context(key));
			return result;
		}
		[HttpPost("remove-his")]
		public void RemoveHis(string key) {
			var cacheMgmt = factory.Get<HisHerData>(nameof(HisCacheMgmt));
			cacheMgmt.Remove(key);
		}
		[HttpPost("reset-his")]
		public void ResetHis() {
			var cacheMgmt = factory.Get<HisHerData>(nameof(HisCacheMgmt));
			cacheMgmt.Reset();
		}

		[HttpGet("her")]
		public async Task<HisHerData> GetHer(string key) {
			var cacheMgmt = factory.Get<HisHerData>(nameof(HerCacheMgmt));
			var result = await cacheMgmt.ExecuteAsync(context => Task.FromResult(new HisHerData(key, Random.Shared.Next(200,300))), new Context(key));
			return result;
		}
		[HttpPost("remove-her")]
		public void RemoveHer(string key) {
			var cacheMgmt = factory.Get<HisHerData>(nameof(HerCacheMgmt));
			cacheMgmt.Remove(key);
		}
		[HttpPost("reset-her")]
		public void ResetHer() {
			var cacheMgmt = factory.Get<HisHerData>(nameof(HerCacheMgmt));
			cacheMgmt.Reset();
		}
	}
}
