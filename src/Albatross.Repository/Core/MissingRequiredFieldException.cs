using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Albatross.Repository.Core
{
    public class MissingRequiredFieldException : Exception
    {
        public const byte Class = 16;
        public const int NotNull = 515;

        public MissingRequiredFieldException() {
        }
        public MissingRequiredFieldException(string msg, Exception err) : base(msg, err) { }

        public static bool Check(SqlException exception) => exception != null && exception.Class == Class && exception.Number == NotNull;
    }
}
