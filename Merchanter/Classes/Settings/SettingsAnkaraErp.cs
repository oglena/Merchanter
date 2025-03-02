namespace Merchanter.Classes.Settings {
    public class SettingsAnkaraErp {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string? company_code { get; set; } = null;
        public string? user_name { get; set; } = null;
        public string? password { get; set; } = null;
        public string? work_year { get; set; } = null;
        public string? url { get; set; } = null;
    }
}
