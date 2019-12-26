using Albatross.Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Model {
	public class Foo:BaseEntity<User>{
		public string Name { get; private set; }
		public int ID { get; internal set; }
	}
}
