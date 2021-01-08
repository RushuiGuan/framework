using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Messages {
	public class BaseEntity {
		public string CreatedBy {get; set;}
		public string ModifiedBy {get; set;}
		public DateTimeOffset CreatedByUTC {get; set;}
		public DateTimeOffset ModifiedByUTC {get; set;}
	}
}
