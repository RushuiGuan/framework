using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Messaging.Configurations {
	public class PublisherConfiguration {
		/// <summary>
		/// The amount of time elapsed to indicate that a subscriber are in the state of lost
		/// </summary>
		public int HeartbeatThreshold { get; set; }
	}
}
