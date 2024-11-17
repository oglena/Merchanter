namespace Merchanter.Classes.Settings {
	public class SettingsProduct {
		public int id { get; set; }
		public int customer_id { get; set; }
		public string default_brand { get; set; } = "DİĞER"; //TODO: this property needs to be moved to SettingsProduct
		public int customer_root_category_id { get; set; } = 1; //TODO: this property needs to be moved to SettingsProduct
		public bool xml_qty_addictive_enable { get; set; } = true; //TODO: this property needs to be moved to SettingsProduct
		public bool is_barcode_required { get; set; } = true; //TODO: this property needs to be moved to SettingsProduct
	}
}
