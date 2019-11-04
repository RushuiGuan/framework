using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;

namespace Albatross.Repository.ByEFCore {
	public static class ErrorHandling {
		public static bool TryConvertError(this DbUpdateException err, out Exception converted) {
			return TryConvertError(err?.InnerException as SqlException, err.Message, out converted);
		}

		public static bool TryConvertError(this SqlException err, string msg, out Exception converted) {
			converted = null;
			if (UniqueKeyViolationException.Check(err)) {
				converted = new UniqueKeyViolationException(err);
				return true;
			} else if (MissingRequiredFieldException.Check(err)) {
				converted = new MissingRequiredFieldException(msg, err);
				return true;
			}

			return false;
		}
	}
}
