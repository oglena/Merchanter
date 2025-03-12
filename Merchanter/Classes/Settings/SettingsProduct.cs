namespace Merchanter.Classes.Settings {
	public class SettingsProduct {
		public int id { get; set; }
		public int customer_id { get; set; }
		public string default_brand { get; set; } = "DİĞER";
		public int customer_root_category_id { get; set; } = 1;
		public bool product_list_filter_source_products { get; set; } = false;
		public bool xml_qty_addictive_enable { get; set; } = true;
		public bool is_barcode_required { get; set; } = true;
	}
}
