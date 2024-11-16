namespace Merchanter.Classes.Settings {
    public class SettingsGeneral {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string company_name { get; set; } = "DEMO FİRMA";
        public decimal rate_TL { get; set; } = 1;
        public decimal rate_USD { get; set; } = 0;
        public decimal rate_EUR { get; set; } = 0;
        public int daysto_ordersync { get; set; } = 7;  //TODO: this property needs to be moved to SettingsOrder
		public int daysto_invoicesync { get; set; } = 7; //TODO: this property needs to be moved to SettingsInvoice
		public string default_brand { get; set; } = "DİĞER";
        public int customer_root_category_id { get; set; } = 1;
        public bool xml_qty_addictive_enable { get; set; } = true;
        public bool is_barcode_required { get; set; } = true;
    }
}
