using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Messaging.Eventing {
	public class SubscriptionManagement {
		Dictionary<string, ISet<string>> topicSubscriptions = new Dictionary<string, ISet<string>>();
		Dictionary<string, ISet<string>> regexPatternSubscriptions = new Dictionary<string, ISet<string>>();
	}
}
