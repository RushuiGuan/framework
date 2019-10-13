using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.CodeGen.CSharp.Conversion {
	public class ConvertDatabaseTableToClass : IConvertObject<Table, Class>
    {
		ConvertSqlTypeToDotNetType getDotNetType;

		public ConvertDatabaseTableToClass(ConvertSqlTypeToDotNetType getDotNetType) {
			this.getDotNetType = getDotNetType;
		}

		public Class Convert(Table table) {
			Class @class = new Class {
				Name = table.Name.Proper(),
				Properties = from item in table.Columns select new Property(item.Name.Proper()) {
					Type = getDotNetType.Convert(item.Type),
					Modifier = AccessModifier.Public,
				},
			};
			return @class;
		}

        object IConvertObject<Table>.Convert(Table from)
        {
            return this.Convert(from);
        }
    }
}
