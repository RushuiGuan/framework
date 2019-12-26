namespace Albatross.CRM.Model {
	public class PurchaseOrderItem { 
		public PurchaseOrder PurchaseOrder { get; private set; }
		public Product Product { get; set; }
		public decimal Unit { get; set; }
		public decimal UnitPrice { get; set; }
	}
}