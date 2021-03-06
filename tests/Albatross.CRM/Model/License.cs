﻿using System.Linq;
using msg = Albatross.CRM.Messages;
using System;
using System.ComponentModel.DataAnnotations;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Albatross.CRM.Repository;

namespace Albatross.CRM.Model {
	public class License : MutableEntity {
		protected License() { }
		public License(msg.License license, string user, IDbSession session) {
		}
		public int LicenseID { get; private set; }

		[StringLength(256)]
		[Required]
		public string Key { get; private set; }

		public int CustomerID { get; private set; }
		public virtual Customer Customer { get; private set; }

		public int ProductID { get; private set; }
		public virtual Product Product { get; private set; }

		public DateTime StartDate {get;private set;}
		public DateTime EndDate {get;private set;}

		public override void Validate(IDbSession session) {
			base.Validate(session);
			if(StartDate > EndDate) {
				throw new ValidationException("Begin date should be before end date");
			}
		}
		public void Update(msg.License license, string user, IProductRepository products, IDbSession session) {
			Product = (from item in products.Items where item.Name == license.Product select item).First();
			StartDate = license.StartDate;
			EndDate = license.EndDate;
			base.CreateOrUpdate(user, session);
		}

		public msg.License CreateDto() {
			return new msg.License {
				CustomerID = CustomerID,
				EndDate = EndDate,
				Key = Key,
				StartDate = StartDate,
			};
		}
	}
}
