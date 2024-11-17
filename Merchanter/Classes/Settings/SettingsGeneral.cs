namespace Merchanter.Classes.Settings {
    public class SettingsGeneral {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string company_name { get; set; } = "DEMO FİRMA";

        #region Currencies
        public decimal rate_TL { get; set; } = 1;
        public decimal rate_USD { get; set; } = 0;
        public decimal rate_EUR { get; set; } = 0; 
        #endregion
    }
}
