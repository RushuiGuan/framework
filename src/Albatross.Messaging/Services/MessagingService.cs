﻿using Albatross.Messaging.DataLogging;
using Albatross.Messaging.Messages;
using NetMQ;
using System;

namespace Albatross.Messaging.Services {
	/// <summary>
	/// A messaging service that runs a zeromq socket and netmq queue.
	/// </summary>
	public interface IMessagingService {
		NetMQSocket Socket { get; }
		IDataLogWriter DataLogger { get; }
		void SubmitToQueue(object message);
	}
}
