﻿using Albatross.Messaging.Commands;
using Albatross.Messaging.Eventing;
using Albatross.Messaging.Eventing.Sub;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleProject.WebApi.Controllers {
	[Route("api/run")]
	[ApiController]
	public class RunController : ControllerBase {
		private readonly ICommandClient commandClient;
		private readonly ISubscriptionClient subscriptionClient;
		private readonly ISubscriber subscriber;

		public RunController(IMyCommandClient commandClient, IMySubscriptionClient subscriptionClient, ISubscriber subscriber) {
			this.commandClient = commandClient;
			this.subscriptionClient = subscriptionClient;
			this.subscriber = subscriber;
		}


		[HttpPost("ping")]
		public Task Ping() => commandClient.Ping();

		[HttpPost("queue-status")]
		public async Task<string> QueueStatus() {
			var result = await commandClient.QueueStatus();
			return JsonSerializer.Serialize(result);
		}

		[HttpPost("sub")]
		public Task Subscribe([FromQuery] string topic) 
			=> subscriptionClient.Subscribe(this.subscriber, topic);

		[HttpPost("unsub")]
		public Task Unsubscribe([FromQuery] string topic) => subscriptionClient.Unsubscribe(this.subscriber, topic);

		[HttpPost("unsub-all")]
		public Task UnsubscribeAll () => subscriptionClient.UnsubscribeAll();
	}
}