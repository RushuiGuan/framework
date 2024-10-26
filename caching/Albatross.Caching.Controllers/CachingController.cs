using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Caching.Controllers {
	[Route("api/caching")]
	[ApiController]
	[Authorize]
	public class CachingController : ControllerBase {
		private readonly ICacheKeyManagement keyMgmt;
		private readonly ICacheProviderAdapter cacheProviderAdapter;

		public CachingController(ICacheKeyManagement keyMgmt, ICacheProviderAdapter cacheProviderAdapter) {
			this.keyMgmt = keyMgmt;
			this.cacheProviderAdapter = cacheProviderAdapter;
		}

		[HttpGet("keys")]
		public IEnumerable<string> Keys() => keyMgmt.FindKeys("*");

		[HttpGet]
		public async Task<string?> Get(string key) {
			var provider = cacheProviderAdapter.Create<string>();
			var result = await provider.TryGetAsync(key, CancellationToken.None, false);
			if (result.Item1) {
				return result.Item2;
			} else {
				return null;
			}
		}

		[HttpPost("remove")]
		public void Remove([FromBody] IEnumerable<string> keys) {
			this.keyMgmt.Remove(keys.ToArray());
		}

		[HttpPost("reset")]
		public void Reset() {
			var keys = this.keyMgmt.FindKeys("*");
			this.keyMgmt.Remove(keys);
		}
	}
}