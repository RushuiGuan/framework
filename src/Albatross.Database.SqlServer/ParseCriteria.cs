using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database.SqlServer{
	/// <summary>
	/// convert wild card * to sql wild card
	/// </summary>
	public class ParseCriteria : IParseCriteria {
		public void Parse(string criteria, out string schema, out string name) {
			schema = null;
			name = null;
			if (!string.IsNullOrEmpty(criteria)) {
				int index = criteria.IndexOf('.');
				if (index == -1) {
					name = criteria.Trim();
				} else {
					schema = criteria.Substring(0, index).Trim();
					name = criteria.Substring(index + 1).Trim();
				}
			}
			if(name == "*") {
				name = null;
			}else if (!string.IsNullOrEmpty(name)) {
				name = name.Replace("*", "%");
			}
			if (schema == "*") {
				schema = null;
			} else if (!string.IsNullOrEmpty(schema)) {
				schema = schema.Replace("*", "%");
			}
		}
	}
}
