﻿using Albatross.Messaging.Services;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {

	public class CommandClient : ICommandClient {
		private readonly CommandClientService service;
		private readonly DealerClient dealerClient;

		public CommandClient(DealerClient dealerClient, CommandClientService service) {
			this.service = service;
			this.dealerClient = dealerClient;
		}
		public Task Ping() => service.Ping(dealerClient);
		public Task<CommandQueueInfo[]> QueueStatus() => service.QueueStatus(dealerClient);
		public Task<ResponseType> Submit<CommandType, ResponseType>(CommandType command)
			where CommandType : notnull
			where ResponseType : notnull => service.Submit<CommandType, ResponseType>(dealerClient, command);

		public Task Submit(object command, bool fireAndForget = true)
			=> service.Submit(dealerClient, command, fireAndForget);
	}
}