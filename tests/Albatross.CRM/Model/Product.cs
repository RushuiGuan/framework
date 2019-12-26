using Albatross.Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Model {
	public class Product :BaseEntity<User>{
		public int ProductID {get;private set;}
		public string Name {get;private set;}
		public string Description {get;private set;}
		public DateTime AvailableDate {get;private set;}

		public ProductCategory Category {			get;private set;		}
	}
}
