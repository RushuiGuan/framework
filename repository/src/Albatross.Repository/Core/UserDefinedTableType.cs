using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Text;
using static Dapper.SqlMapper;

namespace Albatross.Repository.Core
{
    public abstract class UserDefinedTableType<T> : List<SqlDataRecord>
    {
        public UserDefinedTableType(IEnumerable<T> data)
        {
            if (data != null)
            {
                foreach (var item in data)
                {
                    this.Add(Build(item));
                }
            }
        }
        protected abstract SqlDataRecord Build(T data);
        public abstract string CustomTypeName { get; }
        public ICustomQueryParameter Parameter { get { return this.AsTableValuedParameter(CustomTypeName); } }
    }
}
