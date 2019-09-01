using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Repository.Core
{
    public interface ITransaction: IDisposable
    {
        void Commit();
        void Rollback();

        IDbTransaction DbTransaction { get; }
    }
}
