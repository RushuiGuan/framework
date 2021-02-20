namespace Albatross.Config.Core {
	public class ProgramSetting {
		public const string Key = "program";
		public const string WindowsServiceManager = "windows";
		public const string SystemDServiceManager = "systemd";
		/// <summary>
		/// systemd or windows, used by the Worker host
		/// </summary>
		public string ServiceManager { get; set; }
        /// <summary>
        /// Required: name of the application
        /// </summary>
		public string App { get; set; }
        /// <summary>
        /// Optional: the group of the application
        /// </summary>
        public string Group { get; set; }
		public bool IsProd { get; set; }
	}
}
