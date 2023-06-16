using Albatross.Messaging.Commands;
using Albatross.Messaging.Eventing;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleProject.WebApi.Controllers {
	[Route("api/run")]
	[ApiController]
	public class RunController : ControllerBase {
		private readonly ICommandClient commandClient;
		private readonly ISubscriptionClient subscriptionClient;
		private readonly ISubscriber subscriber;

		public RunController(ICommandClient commandClient, ISubscriptionClient subscriptionClient, ISubscriber subscriber) {
			this.commandClient = commandClient;
			this.subscriptionClient = subscriptionClient;
			this.subscriber = subscriber;
		}

		[HttpPost("long-running")]
		public async Task<int> LongRunningCommand([FromQuery] int counter, [FromQuery] int duration) {
			var list = new List<Task<int>>();
			for (int i = 0; i < counter; i++) {
				var task = commandClient.Submit<LongRunningCommand, int>(new LongRunningCommand(duration, i));
				list.Add(task);
			}
			int data = 0;
			foreach(var item in list) {
				data += await item;
			}
			return data / list.Count;
		}

		[HttpPost("math-work")]
		public Task<long> DoMathWork([FromQuery] long counter) {
			var task = commandClient.Submit<DoMathWorkCommand, long>(new DoMathWorkCommand(counter));
			return task;
		}

		[HttpPost("fire-and-forget-math-work")]
		public Task DoFireAndForgetMathWork([FromQuery] long counter) {
			var task = commandClient.Submit<DoMathWorkCommand>(new DoMathWorkCommand(counter), true);
			return task;
		}

		[HttpPost("process-data")]
		public Task<long> ProcessData([FromQuery] long counter) {
			var task = commandClient.Submit<ProcessDataCommand, long>(new ProcessDataCommand(counter));
			return task;
		}

		[HttpPost("unstable")]
		public async Task<int> Unstable([FromQuery] int counter) {
			return await commandClient.Submit<UnstableCommand, int>(new UnstableCommand(counter));
		}

		[HttpPost("fire-and-forget")]
		public Task FireAndForget([FromQuery] int counter, [FromQuery] int? duration) {
			var task = commandClient.Submit<FireAndForgetCommand>(new FireAndForgetCommand(counter, duration), true);
			return task;
		}

		[HttpPost("fire-and-wait")]
		public async Task FireAndWait([FromQuery] int counter, [FromQuery] int? duration) {
			List<Task> list = new List<Task>();
			for (int i = 0; i < counter; i++) {
				var task = commandClient.Submit<FireAndForgetCommand>(new FireAndForgetCommand(i, duration), false);
				list.Add(task);
			}
			await Task.WhenAll(list);
		}

		[HttpPost("ping")]
		public Task Ping() => commandClient.Ping();

		[HttpPost("queue-status")]
		public async Task<string> QueueStatus() {
			var result = await commandClient.QueueStatus();
			return JsonSerializer.Serialize(result);
		}

		[HttpPost("pub")]
		public Task Publish([FromQuery] string topic, [FromQuery] int min, [FromQuery] int max) {
			commandClient.Submit<PublishCommand>(new PublishCommand(topic, min, max), true);
			return Task.CompletedTask;
		}

		[HttpPost("sub")]
		public Task Subscribe([FromQuery] IEnumerable<string> topic) {
			subscriptionClient.Subscribe(this.subscriber, topic.ToArray());
			return Task.CompletedTask;
		}
		[HttpPost("unsub")]
		public Task Unsubscribe([FromQuery] IEnumerable<string> topic) {
			subscriptionClient.Unsubscribe(this.subscriber, topic.ToArray());
			return Task.CompletedTask;
		}

		[HttpPost("play-ping")]
		public Task Ping([FromQuery] int round) {
			return commandClient.Submit<PingCommand>(new PingCommand(round));
		}
		[HttpPost("play-pong")]
		public Task Pong([FromQuery] int round) {
			return commandClient.Submit<PongCommand>(new PongCommand(round));
		}
	}
}
