﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Authentication.Core
{
    public interface IGetCurrentUser
    {
        string Get();
        string Provider { get; }
    }
}
