using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.UnitTest.Model {
	public class BaseEntity {
		protected BaseEntity() { }
		public BaseEntity(int user) {
			CreatedBy = user;
			Created = DateTime.UtcNow;
		}

		public void Update(int user) {
			ModifiedBy = user;
			Modified = DateTime.UtcNow;
		}
		public int CreatedBy { get; set; }
		public int ModifiedBy { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		public DateTimeOffset CreatedUTC => new DateTimeOffset(Created, TimeSpan.Zero);
		public DateTimeOffset ModifiedUTC => new DateTimeOffset(Modified, TimeSpan.Zero);
	}
}
