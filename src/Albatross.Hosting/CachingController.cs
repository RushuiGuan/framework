using Albatross.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Albatross.Hosting {
	[Route("api/caching")]
	[ApiController]
	public class CachingController : ControllerBase {
		private readonly IMemoryCache memoryCache;
		private readonly IMemoryCacheExtended memoryCacheExtended;

		public CachingController(IMemoryCache memoryCache, IMemoryCacheExtended memoryCacheExtended) {
			this.memoryCache = memoryCache;
			this.memoryCacheExtended = memoryCacheExtended;
		}

		[HttpGet("keys")]
		public IEnumerable<string?> Keys() => memoryCacheExtended.Keys.Select(args => Convert.ToString(args));

		[HttpGet]
		public object? Get(string key) => memoryCache.Get(key);

		[HttpPost("envict")]
		public void Envict([FromBody] IEnumerable<string?> keys) {
			foreach(var key in keys) {
				if (!string.IsNullOrEmpty(key)) {
					memoryCache.Remove(key);
				}
			}
		}

		[HttpPost]
		public void Reset() => this.memoryCacheExtended.Reset();
	}
}
