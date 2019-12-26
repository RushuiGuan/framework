using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Messages {
	public class BaseEntityDto {
		public int CreatedBy {get;private set;}
		public int ModifiedBy {get;private set;}
		public DateTimeOffset CreatedByUTC {get;private set;}
		public DateTimeOffset ModifiedByUTC {get;private set;}
	}
}
