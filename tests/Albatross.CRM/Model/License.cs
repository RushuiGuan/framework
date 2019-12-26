using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Albatross.Repository.Core;

namespace Albatross.CRM.Model {
	public class License : BaseEntity<User> {
		protected License() { }
		public License(User user) : base(user) {
		}
		public int LicenseID { get; private set; }
		public int ProductID { get; private set; }
		public int CustomerID { get; private set; }

		[StringLength(256)]
		[Required]
		public string Key { get; private set; }

		public virtual Customer Customer { get; private set; }
		public virtual Product Product { get; private set; }
		public DateTime BeginDate {get;private set;}
		public DateTime EndDate {get;private set;}

		public override void Validate() {
			base.Validate();
			if(BeginDate > EndDate) {
				throw new ValidationException("Begin date should be before end date");
			}
		}
	}
}
