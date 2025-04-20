namespace Merchanter.Classes.Settings
{
    public class SettingsMerchanter
    {
        public int customer_id { get; set; }
        public SettingsGeneral settings { get; set; }
		public SettingsInvoice invoice { get; set; }
		public SettingsProduct product { get; set; }
		public SettingsMagento magento { get; set; }
		public SettingsOrder order { get; set; }
		public SettingsNetsis netsis { get; set; }
        public SettingsEntegra entegra { get; set; }
        public SettingsShipment shipment { get; set; }
		public SettingsN11 n11 { get; set; }
		public SettingsHB hb { get; set; }
		public SettingsTY ty { get; set; }
        public SettingsAnkaraErp ank_erp { get; set; }
        public SettingsIdeasoft ideasoft { get; set; }
        public SettingsGoogle google { get; set; }

        public List<OrderStatus> order_statuses { get; set; }
        public List<PaymentMethod> payment_methods { get; set; }
        public List<ShipmentMethod> shipment_methods { get; set; }
        public List<Integration> integrations { get; set; }
		public List<Platform> platforms { get; set; }
		public List<Work> works { get; set; }
		public List<SyncMapping> sync_mappings { get; set; }

		//TODO: These properties should be moved to a separate class
		public string erp_invoice_ftp_username { get; set; } = string.Empty;
        public string erp_invoice_ftp_password { get; set; } = string.Empty;
        public string erp_invoice_ftp_url { get; set; } = string.Empty;
        public string xml_bogazici_bayikodu { get; set; } = string.Empty;
        public string xml_bogazici_email { get; set; } = string.Empty;
        public string xml_bogazici_sifre { get; set; } = string.Empty;
        public string xml_fsp_url { get; set; } = string.Empty;
        public string xml_koyuncu_url { get; set; } = string.Empty;
        public string xml_oksid_url { get; set; } = string.Empty;
        public string xml_penta_base_url { get; set; } = string.Empty;
        public string xml_penta_customerid { get; set; } = string.Empty;

        public SettingsMerchanter(int _customer_id)
        {
            customer_id = _customer_id;
        }
    }
}
