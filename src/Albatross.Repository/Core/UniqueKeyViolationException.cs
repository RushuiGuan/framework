using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Albatross.Repository.Core
{
    public class UniqueKeyViolationException : Exception
    {
        public const byte Class = 14;
        public const int UniqueConstraint = 2627;
        public const int UniqueIndex = 2601;

        public UniqueKeyViolationException() { }
        public UniqueKeyViolationException(SqlException err) : base(err.Message, err) { }

        public static bool Check(SqlException exception) => exception != null && exception.Class == Class && (exception.Number == UniqueConstraint || exception.Number == UniqueIndex);
    }
}
