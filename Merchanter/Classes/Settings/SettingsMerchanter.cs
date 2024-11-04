namespace Merchanter.Classes.Settings
{
    public class SettingsMerchanter
    {
        public int customer_id { get; set; }
        public int customer_root_category { get; set; }
        public SettingsGeneral settings { get; set; }
        public SettingsMagento magento { get; set; }
        public SettingsNetsis netsis { get; set; }
        public SettingsEntegra entegra { get; set; }
        public SettingsShipment shipment { get; set; }
        public List<OrderStatus> order_statuses { get; set; }
        public List<PaymentMethod> payment_methods { get; set; }
        public List<ShipmentMethod> shipment_methods { get; set; }
        public List<WorkSource> work_sources { get; set; }
        public List<SyncMapping> sync_mappings { get; set; }
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
        public string DefaultBrand { get; set; } = "DİĞER";

        public SettingsMerchanter(int _customer_id)
        {
            customer_id = _customer_id;
        }
    }
}
