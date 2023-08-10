using System;
using System.Runtime.Serialization;

namespace Albatross.Messaging.ReqRep {
	public class NoAvailableWorkerException : Exception {
		public NoAvailableWorkerException(string service) : base($"No avaible worker found for service: {service}") { }
	}
}