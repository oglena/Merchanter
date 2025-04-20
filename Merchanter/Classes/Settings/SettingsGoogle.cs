namespace Merchanter.Classes.Settings {
    public class SettingsGoogle {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string? email { get; set; } = "info@ceresyazilim.com";
        public string? oauth2_clientid { get; set; } = "831405420683-plhqkd9h2u8h5ijavvc6ln1jk27vkf0d.apps.googleusercontent.com";
        public string? oauth2_clientsecret { get; set; } = "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";
        public string? sender_name { get; set; }
        public bool is_enabled { get; set; } = false;
    }
}
