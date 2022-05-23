using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Albatross.Authentication.Core;
using Albatross.Config;
using Albatross.Hosting.Demo.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Registry;
using Serilog;

namespace Albatross.Hosting.Test {
	[Route("api/[controller]")]
	[Authorize]
	public class ValuesController : Controller {
		private readonly ProgramSetting setting;
		private readonly IConfiguration configuration;
		private readonly ILogger<ValuesController> logger;
		private readonly IGetCurrentUser getCurrentUser;
		private readonly IMemoryCache memoryCache;
		private readonly IReadOnlyPolicyRegistry<string> policyRegistry;
		private readonly IHubContext<NotifHub> hubContext;

		public ValuesController(ProgramSetting setting, IConfiguration configuration, ILogger<ValuesController> logger, 
			IGetCurrentUser getCurrentUser, IMemoryCache memoryCache, IReadOnlyPolicyRegistry<string> policyRegistry, IHubContext<NotifHub> hubContext) {
			logger.LogInformation("{class} instance created", nameof(ValuesController));
			this.setting = setting;
			this.configuration = configuration;
			this.logger = logger;
			this.getCurrentUser = getCurrentUser;
			this.memoryCache = memoryCache;
			this.policyRegistry = policyRegistry;
			this.hubContext = hubContext;
		}

		[HttpGet]
		public string Get() {
			return setting.App;
		}

		[HttpGet("test-error")]
		public IEnumerable<string> TestException() {
			throw new Exception("test");
		}

		[HttpGet("settings")]
		public string GetSettings() {
			var setting = this.configuration.GetSection(TestSetting.Key).Get<TestSetting>();
			return $"{setting.Name} - {setting.Count}";
		}

		[HttpGet("current-user")]
		[Authorize]
		public string CurrentUser() => this.getCurrentUser.Get();

		[HttpGet("symbol")]
		public Task<int[]> GetSymbol(int key) {
			var policy = policyRegistry.Get<IAsyncPolicy<int[]>>(SymbolCacheManagement.CacheName);
			return policy.ExecuteAsync(context => GetImportantNumber(key), new Context(key.ToString()));
		}

		[HttpGet("issuer")]
		public Task<string[]> GetText(int key) {
			var policy = policyRegistry.Get<IAsyncPolicy<string[]>>(IssuerCachedMgmt.CacheName);
			return policy.ExecuteAsync(context => GetImportantText(key), new Context(key.ToString()));
		}

		[HttpPost("evict")]
		public void EvictCache(string key) {
			memoryCache.Remove(key);
		}

		[HttpPost("notif")]
		public async Task SendNotif() {
			await hubContext.Clients.All.SendAsync("update", Guid.NewGuid());
		}

		private async Task<int[]> GetImportantNumber(int key) {
			await Task.Delay(TimeSpan.FromSeconds(5));
			logger.LogInformation("GetImportantNumber called {key}", key);
			if (key > 0) {
				int[] result = new int[key];
				for (int i = 0; i < key; i++) {
					result[i] = i + key;
				}
				return result;
			} else {
				return new int[0];
			}
		}

		private async Task<string[]> GetImportantText(int key) {
			await Task.Delay(TimeSpan.FromSeconds(5));
			logger.LogInformation("GetImportantText called {key}", key);
			if (key > 0) {
				string[] result = new string[key];
				for (int i = 0; i < key; i++) {
					result[i] = Convert.ToString(i + key);
				}
				return result;
			} else {
				return new string[0];
			}
		}
	}
}
