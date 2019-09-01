using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Authentication.Core
{
    public interface IGetAccess
    {
        bool HasAccess(string service, string operation);
    }
}
