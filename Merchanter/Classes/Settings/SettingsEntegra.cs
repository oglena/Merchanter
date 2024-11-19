namespace Merchanter.Classes.Settings {
	public class SettingsEntegra {
		public int id { get; set; }
		public int customer_id { get; set; }

		#region Connection Properties
		public string api_url { get; set; } = @"https://";
		public string? api_username { get; set; } = null;
		public string? api_password { get; set; } = null;
		#endregion
	}
}
