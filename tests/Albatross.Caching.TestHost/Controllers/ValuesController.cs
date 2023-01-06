using Polly;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Albatross.Caching.TestHost.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController: ControllerBase {
		private readonly ICacheManagementFactory factory;

		public ValuesController(ICacheManagementFactory factory) {
			this.factory = factory;
		}

		[HttpGet("mine")]
		public async Task<string> GetMine(string key) {
			var cachMgmt = factory.Get<string>(nameof(MyCacheMgmt));
			var result = await cachMgmt.ExecuteAsync(context => Task.FromResult(key.GetHashCode().ToString()), new Context(key));
			return result;
		}
		[HttpPost("remove-mine")]
		public void RemoveMine(string key) {
			var cachMgmt = factory.Get<string>(nameof(MyCacheMgmt));
			cachMgmt.Remove(new Context(key));
		}
		[HttpPost("reset-mine")]
		public void ResetMine() {
			var cachMgmt = factory.Get<string>(nameof(MyCacheMgmt));
			cachMgmt.Reset();
		}

		[HttpGet("yours")]
		public async Task<byte[]> GetYours(string key) {
			var cachMgmt = factory.Get<byte[]>(nameof(YourCacheMgmt));
			var result = await cachMgmt.ExecuteAsync(context => Task.FromResult(Encoding.UTF8.GetBytes(key)), new Context(key));
			return result;
		}
		[HttpPost("remove-yours")]
		public void RemoveYours(string key) {
			var cachMgmt = factory.Get<byte[]>(nameof(YourCacheMgmt));
			cachMgmt.Remove(new Context(key));
		}
		[HttpPost("reset-yours")]
		public void ResetYours() {
			var cachMgmt = factory.Get<byte[]>(nameof(YourCacheMgmt));
			cachMgmt.Reset();
		}

		[HttpGet("his")]
		public async Task<HisHerData> GetHis(string key) {
			var cachMgmt = factory.Get<HisHerData>(nameof(HisCacheMgmt));
			var result = await cachMgmt.ExecuteAsync(context => Task.FromResult(new HisHerData(key, Random.Shared.Next(0, 100))), new Context(key));
			return result;
		}
		[HttpPost("remove-his")]
		public void RemoveHis(string key) {
			var cachMgmt = factory.Get<HisHerData>(nameof(HisCacheMgmt));
			cachMgmt.Remove(new Context(key));
		}
		[HttpPost("reset-his")]
		public void ResetHis() {
			var cachMgmt = factory.Get<HisHerData>(nameof(HisCacheMgmt));
			cachMgmt.Reset();
		}

		[HttpGet("her")]
		public async Task<HisHerData> GetHer(string key) {
			var cachMgmt = factory.Get<HisHerData>(nameof(HerCacheMgmt));
			var result = await cachMgmt.ExecuteAsync(context => Task.FromResult(new HisHerData(key, Random.Shared.Next(200,300))), new Context(key));
			return result;
		}
		[HttpPost("remove-her")]
		public void RemoveHer(string key) {
			var cachMgmt = factory.Get<HisHerData>(nameof(HerCacheMgmt));
			cachMgmt.Remove(new Context(key));
		}
		[HttpPost("reset-her")]
		public void ResetHer() {
			var cachMgmt = factory.Get<HisHerData>(nameof(HerCacheMgmt));
			cachMgmt.Reset();
		}
	}
}
