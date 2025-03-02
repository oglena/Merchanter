namespace Merchanter.Classes.Settings {
    public class SettingsIdeasoft {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string? store_url { get; set; } = null;
        public string? client_id { get; set; } = null;
        public string? client_secret { get; set; } = null;
        public string? refresh_token { get; set; } = null;
        public string? access_token { get; set; } = null;
        public DateTime? update_date { get; set; } = null;
    }
}
