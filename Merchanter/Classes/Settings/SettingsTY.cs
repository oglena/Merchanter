namespace Merchanter.Classes.Settings {
	public class SettingsTY {
		public int id { get; set; }
		public int customer_id { get; set; }
		public string? seller_id { get; set; } = null;
		public string? api_key { get; set; } = null;
		public string? api_secret { get; set; } = null;
	}
}
