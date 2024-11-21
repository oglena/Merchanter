namespace Merchanter.Classes.Settings {
	public class SettingsHB {
		public int id { get; set; }
		public int customer_id { get; set; }
		public string? merchant_id { get; set; } = null;
		public string? token { get; set; } = null;
		public string? user_name { get; set; } = null;
		public string? password { get; set; } = null;
	}
}
