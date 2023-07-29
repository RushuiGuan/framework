﻿using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Eventing {
	public interface ISubscriber {
		public virtual string Name => this.GetType().Name;
		Task DataReceived(string topic, byte[] data);
	}
}
