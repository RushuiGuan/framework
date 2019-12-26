using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Messages {
	public class License {
		public int CustomerID {get;private set;}
		public string Product {get;private set;}
		public string Key {get;private set;}
		public DateTime StartDate {get;private set;}
		public DateTime EndDate {get;private set;}
	}
}
