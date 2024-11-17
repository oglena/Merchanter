namespace Merchanter.Classes.Settings {
	public class SettingsOrder {
		public int id { get; set; }
		public int customer_id { get; set; }
		public int daysto_ordersync { get; set; } = 7;
		public bool is_rewrite_siparis { get; set; } = false;
		public bool siparis_kdvdahilmi { get; set; } = false;
		public string siparis_kargo_sku { get; set; } = "KARGO";
		public string siparis_taksitkomisyon_sku { get; set; } = "KMS-001";
	}
}
