using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Dto {
	public class BaseEntityDto {
		public int CreatedBy { get; set; }
		public int ModifiedBy { get; set; }
		public DateTimeOffset CreatedByUTC { get; set; }
		public DateTimeOffset ModifiedByUTC { get; set; }
	}
}
