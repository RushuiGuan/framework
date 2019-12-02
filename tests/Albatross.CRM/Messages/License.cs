using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Messages {
	public class License {
		public int CustomerID { get; set; }
		public string Product { get; set; }
		public string Key { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
