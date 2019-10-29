﻿using Albatross.Config.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Host.AspNetCore {
    public class AuthorizationSetting: IConfigSetting {
		public const string key = "authorization";

		/// <summary>
		/// BaseUrl of the Authorization Server
		/// </summary>
        public string Authority { get; set; }
		public string TokenUrl { get; set; }
		public string SwaggerClientId { get; set; }
		public string SwaggerClientSecret { get; set; }

		/// <summary>
		/// ApiResource Name
		/// </summary>
		public string Audience { get; set; }

		public void Validate() {
			if (string.IsNullOrEmpty(Authority)) {
				throw new ConfigurationException(this.GetType(), nameof(Authority));
			}
			if (string.IsNullOrEmpty(Audience)) {
				throw new ConfigurationException(this.GetType(), nameof(Audience));
			}
		}
    }
}
