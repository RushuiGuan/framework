﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.Core {
	public interface IGetConfig<T> {
		T Get();
	}
}
