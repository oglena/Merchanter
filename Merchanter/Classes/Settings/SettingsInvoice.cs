namespace Merchanter.Classes.Settings {
	public class SettingsInvoice {
		public int id { get; set; }
		public int customer_id { get; set; }
		public int daysto_invoicesync { get; set; } = 7; 
	}
}
