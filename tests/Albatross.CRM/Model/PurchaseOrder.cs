using System.Collections.Generic;

namespace Albatross.CRM.Model {
	public class PurchaseOrder {
		public Customer Customer { get; private set; }
		public ICollection<PurchaseOrderItem> Items { get; private set; } = new List<PurchaseOrderItem>();
	}
}