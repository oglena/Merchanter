namespace Merchanter.Classes.Settings {
	public class SettingsInvoice {
		public int id { get; set; }
		public int customer_id { get; set; }
		public int daysto_invoicesync { get; set; } = 7;
		public string? erp_invoice_ftp_username { get; set; } = null;
		public string? erp_invoice_ftp_password { get; set; } = null;
		public string? erp_invoice_ftp_url { get; set; } = null;
	}
}
